using Sparcopt.PokeGo.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sparcopt.PokeGo.Api.Enums;

namespace Sparcopt.PokeGo.Demo
{
    public class ClientConfig : IConfiguration
    {
        public AuthType AuthenticationType { get; set; }

        public double DefaultAltitude { get; set; }

        public double DefaultLatitude { get; set; }

        public double DefaultLongitude { get; set; }

        public string LoginId { get; set; }

        public string LoginPassword { get; set; }
    }
}
