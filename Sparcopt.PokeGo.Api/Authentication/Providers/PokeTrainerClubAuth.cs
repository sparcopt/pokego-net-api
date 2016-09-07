using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sparcopt.PokeGo.Api.Authentication.Providers.Data;
using Sparcopt.PokeGo.Api.Enums;
using Sparcopt.PokeGo.Api.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Sparcopt.PokeGo.Api.Authentication.Providers
{
    internal static class PokeTrainerClubAuth
    {
        public static async Task<AccessToken> Login(string username, string password)
        {
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip,
                AllowAutoRedirect = false
            };

            using (var httpClient = new HttpClient(new RetryHandler(handler)))
            {
                var loginData = await GetLoginData(httpClient);
                var ticketId = await PerformLogin(httpClient, loginData, username, password);     

                if (ticketId == null)
                    throw new Exception(); //TODO: ptc offline exception

                var token = await PerformOAuthLogin(httpClient, ticketId);
                return token;          
            }     
        }

        private static async Task<LoginParams> GetLoginData(HttpClient httpClient)
        {
            var sessionResponse = await httpClient.GetAsync(Constants.PtcLoginUrl);
            if (!sessionResponse.IsSuccessStatusCode) //Switch to internal server error?
                throw new Exception(); //TODO: exception

            var loginData = JsonConvert.DeserializeObject<LoginParams>(sessionResponse.Content.ReadAsStringAsync().Result);

            return loginData;
        }

        private static async Task<string> PerformLogin(HttpClient httpClient, LoginParams loginData, string username, string password)
        {
            var loginResponse = await httpClient.PostAsync(Constants.PtcLoginUrl,
                    new FormUrlEncodedContent(new Dictionary<string, string>
                        {
                            { "lt", loginData.Lt },
                            { "execution", loginData.Execution },
                            { "_eventId", "submit" },
                            { "username", username },
                            { "password", password }
                        }));

            var content = await loginResponse.Content.ReadAsStringAsync();

            if (loginResponse.Headers.Location == null)
            {
                var loginResponseData = JObject.Parse(content);
                var loginResponseErrors = (JArray)loginResponseData["errors"];

                if (loginResponse.StatusCode == HttpStatusCode.OK && !loginResponse.Headers.Contains("Set-cookies"))
                    throw new Exception(); //TODO: account not verified exception with loginResponseErrors

                throw new Exception(); //TODO: ptc offline exception
            }

            var ticketId = HttpUtility.ParseQueryString(loginResponse.Headers.Location.Query)["ticket"];
            return ticketId;
        }

        private static async Task<AccessToken> PerformOAuthLogin(HttpClient httpClient, string ticketId)
        {
            var tokenResponse = await httpClient.PostAsync(Constants.PtcLoginOAuthUrl,
                new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                            { "client_id", "mobile-app_pokemon-go" },
                            { "redirect_uri",  "https://www.nianticlabs.com/pokemongo/error" },
                            { "client_secret", "w8ScCUXJQc6kXKw8FiOhd8Fixzht18Dq3PEVkUCP5ZPxtgyWsbTvWHFLm2wNY0JR" },
                            { "grant_type", "refresh_token" },
                            { "code", ticketId}
                    }));

            var tokenData = await tokenResponse.Content.ReadAsStringAsync();

            //TODO: validate response?

            var token = new AccessToken
            {
                Token = HttpUtility.ParseQueryString(tokenData)["access_token"],
                ExpireDate = DateTime.Now.AddSeconds(int.Parse(HttpUtility.ParseQueryString(tokenData)["expires"])),
                AuthenticationType = AuthType.PokemonTrainerClub
            };

            return token;
        } 
    }
}
