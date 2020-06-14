using System;

using Polly;

using RestSharp;

namespace Inverter.GoodWe
{
    public class Observe
    {
        public Action LogAuthentication { get; set; }

        public Action<DelegateResult<IRestResponse>, int, TimeSpan> OnRetry { get; set; }
    }
}