using Sparcopt.PokeGo.Api.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparcopt.PokeGo.Api.Authentication.Providers.Data
{
    public class AccessToken
    {
        public string Token { get; internal set; }

        public DateTime ExpireDate { get; internal set; }

        public AuthType AuthenticationType { get; internal set; }

        public bool IsExpired => DateTime.Now > ExpireDate;
    }
}
