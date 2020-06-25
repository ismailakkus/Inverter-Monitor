using System;

using Inverter.GoodWe.Login;

using RestSharp;

namespace Inverter.GoodWe.Exceptions
{
    public class AuthenticationFailed : Exception
    {
        private AuthenticationFailed(string message) : base(message)
        {
        }

        private AuthenticationFailed(string message, Exception exception) : base(message, exception)
        {
        }

        internal static AuthenticationFailed Create(LoginResponse response)
            => new AuthenticationFailed($"Authentication failed with code: {response.code} and message: {response.msg}");

        public static AuthenticationFailed Create(IRestResponse response)
            => new AuthenticationFailed($"Authentication request unsuccessful: {response.ErrorMessage}", response.ErrorException);
    }
}