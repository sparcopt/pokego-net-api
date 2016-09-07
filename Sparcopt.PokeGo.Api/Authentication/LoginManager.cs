using POGOProtos.Networking.Requests;
using POGOProtos.Networking.Requests.Messages;
using Sparcopt.PokeGo.Api.Authentication.Providers;
using Sparcopt.PokeGo.Api.Enums;
using Sparcopt.PokeGo.Api.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Sparcopt.PokeGo.Api.Domain;

namespace Sparcopt.PokeGo.Api.Authentication
{
    public class LoginManager : RpcBase
    {

        internal LoginManager(Session session)
            : base(session)
        {
        }

        public async Task Authenticate()
        {
            switch(_session.Configuration.AuthenticationType)
            {
                case (AuthType.GoogleAuth):
                    //_session.AccessToken = await GoogleAuth.Login(_session.Configuration.LoginId, _session.Configuration.LoginPassword);
                    break;
                case (AuthType.PokemonTrainerClub):
                    _session.AccessToken = await PokeTrainerClubAuth.Login(_session.Configuration.LoginId, _session.Configuration.LoginPassword);
                    break;
            }

            await SendInitialRequests();
        }

        private async Task SendInitialRequests()
        {
            var getPlayerMessage = new GetPlayerMessage();
            var getHatchedEggsMessage = new GetHatchedEggsMessage();

            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var lastTimeStamp = Convert.ToInt64((DateTime.UtcNow - epoch).TotalMilliseconds);

            var getInventoryMessage = new GetInventoryMessage
            {
                LastTimestampMs = lastTimeStamp
            };
            var checkAwardedBadgesMessage = new CheckAwardedBadgesMessage();
            var downloadSettingsMessage = new DownloadSettingsMessage
            {
                Hash = "05daf51635c82611d1aac95c0b051d3ec088a930"
            };

            var serverRequest = RequestGenerator.GetInitialRequestEnvelope(
                new Request
                {
                    RequestType = RequestType.GetPlayer,
                    RequestMessage = getPlayerMessage.ToByteString()
                }, new Request
                {
                    RequestType = RequestType.GetHatchedEggs,
                    RequestMessage = getHatchedEggsMessage.ToByteString()
                }, new Request
                {
                    RequestType = RequestType.GetInventory,
                    RequestMessage = getInventoryMessage.ToByteString()
                }, new Request
                {
                    RequestType = RequestType.CheckAwardedBadges,
                    RequestMessage = checkAwardedBadgesMessage.ToByteString()
                }, new Request
                {
                    RequestType = RequestType.DownloadSettings,
                    RequestMessage = downloadSettingsMessage.ToByteString()
                });

            var serverResponse = await PostProto<Request>(Constants.RpcUrl, serverRequest);

            if (serverResponse.AuthTicket == null)
            {
                _session.AccessToken.Token = null;
                //throw new AccessTokenExpiredException();
            }

            _session.AuthTicket = serverResponse.AuthTicket;
            _session.ApiUrl = serverResponse.ApiUrl;
        }
    }
}
