using System;
using System.Threading.Tasks;

using Inverter.GoodWe.Exceptions;
using Inverter.GoodWe.Login;

using RestSharp;

namespace Inverter.GoodWe
{
    internal class Authenticator
    {
        private readonly Action _authenticate;
        private readonly Func<DateTimeOffset> _dateTimeProvider;
        private readonly Func<Uri, dynamic, Data, Task<IRestResponse>> _requestFactory;
        private readonly GoodWeSettings _settings;

        private DateTimeOffset _lastUpdated = DateTimeOffset.MinValue;
        private LoginResponse _response;

        public Authenticator(GoodWeSettings settings,
                             Func<DateTimeOffset> dateTimeProvider,
                             Func<Uri, dynamic, Data, Task<IRestResponse>> requestFactory,
                             Action authenticate = null)
        {
            _settings = settings;
            _dateTimeProvider = dateTimeProvider;
            _requestFactory = requestFactory;
            _authenticate = authenticate;
        }

        public async Task<(Data token, string baseUri)> Authentication()
        {
            var timeToLiveElapsed = _lastUpdated.Add(_settings.AuthenticationTimeToLive) <= _dateTimeProvider();

            if(_response != null && !timeToLiveElapsed)
                return (_response.data, _response.api);

            _authenticate?.Invoke();

            _lastUpdated = _dateTimeProvider();
            var payload = new
                          {
                              account = _settings.Username,
                              pwd = _settings.Password
                          };
            var result = await _requestFactory(_settings.AuthenticationUri,
                                               payload,
                                               Data.CreateEmpty)
                             .ConfigureAwait(false);
            _response = LoginResponse.From(result.Content);

            if(_response.hasError)
                throw AuthenticationFailed.Create(_response);

            return (_response.data, _response.api);
        }
    }
}