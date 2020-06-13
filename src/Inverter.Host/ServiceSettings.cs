using System;

namespace Inverter.Host
{
    internal class ServiceSettings
    {
        public TimeSpan Interval { get; } = TimeSpan.FromMinutes(1);
    }
}