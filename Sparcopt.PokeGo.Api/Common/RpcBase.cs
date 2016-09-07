using Google.Protobuf;
using POGOProtos.Networking.Envelopes;
using Sparcopt.PokeGo.Api.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparcopt.PokeGo.Api.Domain
{
    public class RpcBase
    {
        protected Session _session;

        protected RequestGenerator RequestGenerator => 
            new RequestGenerator(
                _session.AccessToken.Token, 
                _session.Configuration.AuthenticationType, 
                _session.Configuration.DefaultLatitude, 
                _session.Configuration.DefaultLongitude, 
                _session.Configuration.DefaultAltitude, 
                _session.AuthTicket);


        protected RpcBase(Session session)
        {
            _session = session;
        }

        protected async Task<ResponseEnvelope> PostProto<TRequest>(string url, RequestEnvelope requestEnvelope)
            where TRequest : IMessage<TRequest>
        {
            return await _session.RpcHttpClient.PostProto<TRequest>(url, requestEnvelope);
        } 
    }
}
