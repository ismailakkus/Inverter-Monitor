using System;
using System.Threading.Tasks;

using Inverter.GoodWe.Exceptions;
using Inverter.GoodWe.Login;

using RestSharp;

namespace Inverter.GoodWe
{
    internal class Authenticator
    {
        private const int authenticationErrorCode = 100005;

        private readonly Action _authenticate;
        private readonly Func<DateTimeOffset> _dateTimeProvider;
        private readonly Func<Uri, dynamic, Data, Task<IRestResponse>> _executeRequest;
        private readonly GoodWeSettings _settings;

        private DateTimeOffset _lastUpdated = DateTimeOffset.MinValue;
        private LoginResponse _response;

        public Authenticator(GoodWeSettings settings,
                             Func<DateTimeOffset> dateTimeProvider,
                             Func<Uri, dynamic, Data, Task<IRestResponse>> executeRequest,
                             Action authenticate = null)
        {
            _settings = settings;
            _dateTimeProvider = dateTimeProvider;
            _executeRequest = executeRequest;
            _authenticate = authenticate;
        }

        private bool _forceAuthenticate;

        public void ForceAuthentication()
        {
            _forceAuthenticate = true;
        }

        public async Task<(Data token, string baseUri)> Authenticate()
        {
            var timeToLiveElapsed = _lastUpdated.Add(_settings.AuthenticationTimeToLive) <= _dateTimeProvider();

            if(!_forceAuthenticate && _response != null && !timeToLiveElapsed)
                return (_response.data, _response.api);

            _forceAuthenticate = false;
            _authenticate?.Invoke();

            _lastUpdated = _dateTimeProvider();
            var payload = new
                          {
                              account = _settings.Username,
                              pwd = _settings.Password
                          };
            var result = await _executeRequest(_settings.AuthenticationUri,
                                               payload,
                                               Data.CreateEmpty)
                             .ConfigureAwait(false);

            if(!result.IsSuccessful)
                throw AuthenticationFailed.Create(result);

            _response = LoginResponse.From(result.Content);

            if(_response.hasError || _response.code == authenticationErrorCode)
                throw AuthenticationFailed.Create(_response);

            return (_response.data, _response.api);
        }
    }
}