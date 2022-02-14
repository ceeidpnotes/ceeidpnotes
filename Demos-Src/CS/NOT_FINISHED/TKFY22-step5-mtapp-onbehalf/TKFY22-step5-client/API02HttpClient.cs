using Microsoft.Identity.Web;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;

namespace fy21_simplemtapp
{
    public class API02HttpClient
    {
        private HttpClient _clt;

        private ITokenAcquisition _tokenAcquisition;
        private string _scope;
        private string _baseURL;
        private string _redirectUri;
        private string _apiRedirectUri;

        public API02HttpClient(ITokenAcquisition tokenAcquisition, HttpClient client, IConfiguration configuration)
        {
            _clt = client;
            _tokenAcquisition = tokenAcquisition;
            _scope = configuration["Api02:Scope"];
            _baseURL = configuration["Api02:BaseURL"];
            _redirectUri = configuration["RedirectUri"];
            _apiRedirectUri = configuration["Api02:AdminConsentRedirectApi"];
            _clt.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<string> CallApi()
        {
            await prepareAuthenticatedClient();
            var response = await _clt.GetAsync(_baseURL);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }

        private async Task prepareAuthenticatedClient()
        {
            var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { _scope });
            Debug.WriteLine($"access token-{accessToken}");
            _clt.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }

}
