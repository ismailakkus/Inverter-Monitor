using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Inverter.GoodWe.PowerStationData;
using Inverter.GoodWe.Systems;
using Inverter.Interfaces;

using RestSharp;

using Data=Inverter.GoodWe.Login.Data;

namespace Inverter.GoodWe
{
    internal class GoodWeRepository : Inverters
    {
        private readonly Authenticator _authenticator;
        private readonly Func<Uri, dynamic, Data, Task<IRestResponse>> _clientExecutionFactory;
        private readonly Observe _observe;

        public GoodWeRepository(Func<Uri, dynamic, Data, Task<IRestResponse>> clientExecutionFactory,
                                Func<DateTimeOffset> utcDateTimeNowProvider,
                                GoodWeSettings settings,
                                Observe observe)
        {
            _clientExecutionFactory = clientExecutionFactory;
            _observe = observe;

            _authenticator = new Authenticator(settings,
                                               utcDateTimeNowProvider,
                                               clientExecutionFactory,
                                               observe.LogAuthentication);
        }

        public async Task<Measurement> LatestMeasurement(InverterId id)
        {
            var response = await PowerStationData(id).ConfigureAwait(false);

            var info = response.data.inverter.Single(i => i.invert_full.powerstation_id == id).invert_full;

            return From(info);

            static Measurement From(Invert_Full invertFull)
            {
                var fault = FromFault(invertFull.fault_messge);
                var createdAt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(invertFull.last_time);
                return new Measurement(createdAt,
                    invertFull.pac,
                    invertFull.tempperature,
                    fault);
            }

            static Fault? FromFault(float fault)
            {
                if(fault == 0)
                    return null;

                return Fault.Unknown;
            }
        }

        public async Task<IReadOnlyList<Inverter>> All()
        {
            var response = await GetPowerStationData().ConfigureAwait(false);

            return response.data.Select(From).ToList();

            static Inverter From(Datum datum)
                => new Inverter(datum.powerstation_id);
        }

        private async Task<PowerStationResponse> PowerStationData(string powerStationId)
        {
            var (token, baseUri) = await _authenticator.Authenticate().ConfigureAwait(false);

            const string method = "v1/PowerStation/GetMonitorDetailByPowerstationId";
            var uri = new Uri($"{baseUri}{method}");
            var response = await _clientExecutionFactory(uri, new {powerStationId}, token).ConfigureAwait(false);

            if(!ShouldReAuthenticate(response.Content))
                return PowerStationResponse.From(response.Content);

            _observe.ReAuthenticating?.Invoke();
            _authenticator.ForceAuthentication();
            return await PowerStationData(powerStationId);
        }

        private async Task<SystemResponse> GetPowerStationData()
        {
            var (token, baseUri) = await _authenticator.Authenticate().ConfigureAwait(false);

            const string method = "PowerStationMonitor/QueryPowerStationMonitorForApp";
            var uri = new Uri($"{baseUri}{method}");
            var payload = new
                          {
                              page_size = 50,
                              orderby = "",
                              powerstation_status = "",
                              key = "",
                              page_index = "1",
                              powerstation_id = "",
                              powerstation_type = ""
                          };
            var response = await _clientExecutionFactory(uri, payload, token).ConfigureAwait(false);

            if(!ShouldReAuthenticate(response.Content))
                return SystemResponse.From(response.Content);

            _observe.ReAuthenticating?.Invoke();
            _authenticator.ForceAuthentication();
            return await GetPowerStationData();
        }

        private static bool ShouldReAuthenticate(string response)
        {
            const string reAuthenticateCode = "100002";
            return response.Contains(reAuthenticateCode);
        }
    }
}