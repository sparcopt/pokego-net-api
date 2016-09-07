using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparcopt.PokeGo.Api.Authentication.Providers.Data
{
    internal struct LoginParams
    {
        [JsonProperty("lt", Required = Required.Always)]
        public string Lt { get; set; }

        [JsonProperty("execution", Required = Required.Always)]
        public string Execution { get; set; }
    }
}
