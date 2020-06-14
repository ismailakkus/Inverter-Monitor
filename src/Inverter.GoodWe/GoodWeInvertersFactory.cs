using System;

using Inverter.Interfaces;

namespace Inverter.GoodWe
{
    public static class GoodWeInvertersFactory
    {
        public static Inverters Build(GoodWeSettings settings,
                                      Observe observe,
                                      Func<DateTimeOffset> utcDateTimeNow)
            => new GoodWeRepository(new ResilientRestClient(observe.OnRetry).Execute, utcDateTimeNow, settings, observe);
    }
}