using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using Inverter.GoodWe.Login;

using Polly;
using Polly.Retry;

using RestSharp;

namespace Inverter.GoodWe
{
    internal class ResilientRestClient
    {
        private static readonly IEnumerable<HttpStatusCode> _httpStatusCodesWorthRetrying = new List<HttpStatusCode>
                                                                                            {
                                                                                                HttpStatusCode.RequestTimeout,
                                                                                                HttpStatusCode.InternalServerError,
                                                                                                HttpStatusCode.BadGateway,
                                                                                                HttpStatusCode.ServiceUnavailable,
                                                                                                HttpStatusCode.GatewayTimeout
                                                                                            };

        private readonly IRestClient _restClient;
        private readonly AsyncRetryPolicy<IRestResponse> _result;

        public ResilientRestClient(Action<DelegateResult<IRestResponse>, int, TimeSpan> onRetry = null)
        {
            _restClient = new RestClient();
            _result = Policy.Handle<HttpRequestException>()
                            .OrResult<IRestResponse>(r => _httpStatusCodesWorthRetrying.Contains(r.StatusCode))
                            .WaitAndRetryForeverAsync(retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                                                      (result, attempt, timeSpan) => { onRetry?.Invoke(result, attempt, timeSpan); });
        }

        public async Task<IRestResponse> Execute(Uri uri,
                                                 dynamic data,
                                                 Data authentication)
        {
            IRestRequest request = new RestRequest(uri, Method.POST);
            request.AddHeader("token", JsonSerializer.Serialize(authentication));
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json",
                                 JsonSerializer.Serialize(data),
                                 ParameterType.RequestBody);

            var result = await _result.ExecuteAndCaptureAsync(() => _restClient.ExecuteAsync(request))
                                      .ConfigureAwait(false);

            return result.Result;
        }
    }
}