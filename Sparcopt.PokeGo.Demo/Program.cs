using Sparcopt.PokeGo.Api;
using Sparcopt.PokeGo.Api.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparcopt.PokeGo.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAuthDemo().Wait();
        }

        private static async Task RunAuthDemo()
        {
            try
            {
                var config = new ClientConfig
                {
                    AuthenticationType = AuthType.PokemonTrainerClub,
                    DefaultLatitude = 43.0909305,
                    DefaultLongitude = -73.498936,
                    DefaultAltitude = 0.0,
                    LoginId = "",
                    LoginPassword = ""
                };

                var session = new Session(config);
                await session.LoginManager.Authenticate();
            }
            catch(Exception ex)
            {

            }
        }
    }
}
