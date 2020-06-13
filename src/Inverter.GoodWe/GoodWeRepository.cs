using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
        private readonly Func<Task<(Data token, string baseUri)>> _authenticationProvider;
        private readonly Func<IRestClient> _restClientFactory;

        public GoodWeRepository(Func<IRestClient> restClientFactory,
                                Func<DateTimeOffset> utcDateTimeNowProvider,
                                GoodWeSettings settings,
                                Observe observe)
        {
            _restClientFactory = restClientFactory;

            var authenticator = new Authenticator(settings,
                                                  utcDateTimeNowProvider,
                                                  Execute,
                                                  observe.LogAuthentication);
            _authenticationProvider = () => authenticator.Authentication();
        }

        public async Task<Measurement> LatestMeasurement(InverterId id)
        {
            var response = await PowerStationData(id).ConfigureAwait(false);

            var info = response.data.inverter.Single(i => i.invert_full.powerstation_id == id).invert_full;

            return From(info);

            Measurement From(Invert_Full invertFull)
            {
                var createdAt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(invertFull.last_time);
                return new Measurement(createdAt, invertFull.pac, invertFull.tempperature);
            }
        }

        public async Task<IReadOnlyList<Inverter>> All()
        {
            var response = await GetPowerStationData().ConfigureAwait(false);

            return response.data.Select(From).ToList();

            Inverter From(Datum datum)
            {
                return new Inverter(datum.powerstation_id);
            }
        }

        private async Task<PowerStationResponse> PowerStationData(string powerStationId)
        {
            var (token, baseUri) = await _authenticationProvider().ConfigureAwait(false);

            const string method = "v1/PowerStation/GetMonitorDetailByPowerstationId";
            var uri = new Uri($"{baseUri}{method}");
            var response = await Execute(uri, new {powerStationId}, token).ConfigureAwait(false);
            
            return PowerStationResponse.From(response.Content);
        }

        private async Task<SystemResponse> GetPowerStationData()
        {
            var (token, baseUri) = await _authenticationProvider().ConfigureAwait(false);

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
            var response = await Execute(uri, payload, token).ConfigureAwait(false);
            
            return SystemResponse.From(response.Content);
        }

        private Task<IRestResponse> Execute(Uri uri,
                                            dynamic data,
                                            Data authentication)
        {
            var request = new RestRequest(uri, Method.POST);
            request.AddHeader("token", JsonSerializer.Serialize(authentication));
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json",
                                 JsonSerializer.Serialize(data),
                                 ParameterType.RequestBody);
            return _restClientFactory().ExecuteAsync(request);
        }
    }
}