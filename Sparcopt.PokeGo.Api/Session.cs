using POGOProtos.Networking.Envelopes;
using Sparcopt.PokeGo.Api.Authentication;
using Sparcopt.PokeGo.Api.Authentication.Providers.Data;
using Sparcopt.PokeGo.Api.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparcopt.PokeGo.Api
{
    public class Session
    {
        public LoginManager LoginManager { get; internal set; }

        public IConfiguration Configuration { get; }
        public AccessToken AccessToken { get; internal set; }


        internal AuthTicket AuthTicket { get; set; }
        internal string ApiUrl { get; set; }
        internal readonly RpcHttpClient RpcHttpClient = new RpcHttpClient();

        public Session(IConfiguration configuration)
        {
            Configuration = configuration;

            LoginManager = new LoginManager(this);
        }
    }
}
