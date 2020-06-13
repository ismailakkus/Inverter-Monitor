using System;

namespace Inverter.GoodWe
{
    public class GoodWeSettings
    {
        public string Username { get; }
        public string Password { get; }

        public Uri AuthenticationUri { get; }
            = new Uri("https://globalapi.sems.com.cn/api/v1/Common/CrossLogin");

        public TimeSpan AuthenticationTimeToLive { get; } = TimeSpan.FromHours(1);
    }
}