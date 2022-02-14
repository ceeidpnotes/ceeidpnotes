using Microsoft.Identity.Web;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;

namespace fy21_simplemtapp
{
    public class API01HttpClient
    {
        private IConfiguration _configuration;
        private HttpClient _clt;

        private ITokenAcquisition _tokenAcquisition;
        private string _baseURL;
        private string _redirectUri;
        private string _apiRedirectUri;

        public API01HttpClient(ITokenAcquisition tokenAcquisition, HttpClient client, IConfiguration configuration)
        {
            _configuration = configuration;
            _clt = client;
            _tokenAcquisition = tokenAcquisition;
            _baseURL = configuration["Api01:BaseURL"];
            _redirectUri = configuration["RedirectUri"];
            _apiRedirectUri = configuration["Api01:AdminConsentRedirectApi"];
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
            var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { _configuration["Api01:Scope"] });
            Debug.WriteLine($"access token-{accessToken}");
            _clt.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }

}
