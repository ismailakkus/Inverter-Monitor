using System;

namespace Inverter.GoodWe
{
    public class GoodWeSettings
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public Uri AuthenticationUri { get; set; }
            = new Uri("https://globalapi.sems.com.cn/api/v1/Common/CrossLogin");

        public TimeSpan AuthenticationTimeToLive { get; set; } = TimeSpan.FromHours(1);
    }
}