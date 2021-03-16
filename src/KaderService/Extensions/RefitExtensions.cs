using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace KaderService.Extensions
{
    public class AuthorizationMessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancelToken)
        {
            HttpRequestHeaders headers = request.Headers;

            // If you have the following attribute in your interface, the authorization header will be "Bearer", not null.
            // [Headers("Authorization: Bearer")]
            AuthenticationHeaderValue authHeader = headers.Authorization;

            if (authHeader != null)
            {
                headers.Authorization = new AuthenticationHeaderValue(authHeader.Scheme, "TestToken");
            }

            return await base.SendAsync(request, cancelToken);
        }
    }
}
