using Microsoft.Graph;
using System.Net.Http.Headers;

namespace TKFY22_step3_stapp_admincosmos
{
    public class GraphHelper
    {
    }
    public class GraphServiceClientFactory
    {
        public static GraphServiceClient GetAuthenticatedGraphClient(Func<Task<string>> acquireAccessToken,
                                                                                 string baseUrl = null)
        {

            return new GraphServiceClient(baseUrl, new CustomAuthenticationProvider(acquireAccessToken));
        }
    }

    class CustomAuthenticationProvider : IAuthenticationProvider
    {
        public CustomAuthenticationProvider(Func<Task<string>> acquireTokenCallback)
        {
            acquireAccessToken = acquireTokenCallback;
        }

        private Func<Task<string>> acquireAccessToken;

        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            string accessToken = await acquireAccessToken.Invoke();

            // Append the access token to the request.
            request.Headers.Authorization = new AuthenticationHeaderValue(
                Constants.BearerAuthorizationScheme, accessToken);
        }
    }

}
