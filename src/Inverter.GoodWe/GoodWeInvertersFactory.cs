using System;

using Inverter.Interfaces;

using RestSharp;

namespace Inverter.GoodWe
{
    public static class GoodWeInvertersFactory
    {
        private static IRestClient Create()
            => new RestClient();

        public static Inverters Build(GoodWeSettings settings,
                                      Observe observe,
                                      Func<DateTimeOffset> utcDateTimeNow)
            => new GoodWeRepository(Create, utcDateTimeNow, settings, observe);
    }
}