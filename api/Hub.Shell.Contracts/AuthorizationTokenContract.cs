using System;

namespace Hub.Shell.Contracts
{
    public class AuthorizationTokenContract
    {
        public string Value { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresUtc { get; set; }
    }
}