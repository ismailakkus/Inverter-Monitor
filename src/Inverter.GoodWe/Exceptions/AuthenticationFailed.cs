using System;

using Inverter.GoodWe.Login;

namespace Inverter.GoodWe.Exceptions
{
    public class AuthenticationFailed : Exception
    {
        private AuthenticationFailed(string message) : base(message)
        {
        }

        internal static AuthenticationFailed Create(LoginResponse response)
            => new AuthenticationFailed($"Authentication failed with code: {response.code} and message: {response.msg}");
    }
}