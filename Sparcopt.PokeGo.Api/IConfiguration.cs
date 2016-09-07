using Sparcopt.PokeGo.Api.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparcopt.PokeGo.Api
{
    public interface IConfiguration
    {
        string LoginId { get; set; }
        string LoginPassword { get; set; }
        AuthType AuthenticationType { get; set; }
        double DefaultLatitude { get; set; }
        double DefaultLongitude { get; set; }
        double DefaultAltitude { get; set; }

    }
}
