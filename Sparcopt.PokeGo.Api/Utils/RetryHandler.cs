using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sparcopt.PokeGo.Api.Utils
{
    internal class RetryHandler : DelegatingHandler
    {
        private const int MaxRetries = 10;

        public RetryHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            for (var i = 0; i <= MaxRetries; i++)
            {
                try
                {
                    var response = await base.SendAsync(request, cancellationToken);
                    //TODO: add delay here because of limits?
                    if (response.StatusCode == HttpStatusCode.BadGateway || response.StatusCode == HttpStatusCode.InternalServerError)
                        throw new Exception(); //todo: proper implementation

                    return response;
                }
                catch (Exception ex)
                {
                    if (i < MaxRetries)
                    {
                        await Task.Delay(1000, cancellationToken);
                        continue;
                    }
                }
            }

            return null;
        }
    }
}
