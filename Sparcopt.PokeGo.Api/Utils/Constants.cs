using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparcopt.PokeGo.Api.Utils
{
    internal static class Constants
    {
        public const string PtcLoginUrl =
            "https://sso.pokemon.com/sso/login?service=https%3A%2F%2Fsso.pokemon.com%2Fsso%2Foauth2.0%2FcallbackAuthorize";
        public const string PtcLoginOAuthUrl = "https://sso.pokemon.com/sso/oauth2.0/accessToken";
        public const string RpcUrl = @"https://pgorelease.nianticlabs.com/plfe/rpc";
    }
}
