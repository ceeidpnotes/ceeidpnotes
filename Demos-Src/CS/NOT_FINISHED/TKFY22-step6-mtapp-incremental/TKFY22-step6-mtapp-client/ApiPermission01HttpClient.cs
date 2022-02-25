using Microsoft.Identity.Web;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;

namespace fy21_simplemtapp
{
    public class ApiPermission01HttpClient
    {
        private HttpClient _clt;

        private ITokenAcquisition _tokenAcquisition;
        private string _baseURL;

        public ApiPermission01HttpClient(ITokenAcquisition tokenAcquisition, HttpClient client, IConfiguration configuration)
        {
            _clt = client;
            _tokenAcquisition = tokenAcquisition;
            _baseURL = configuration["ApiPermission01:BaseURL"];
            _clt.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<string> CallApi()
        {
            await prepareAuthenticatedClient(new string[] { "api://bc36543f-efc2-410b-ad6c-9826ceaceedf/ApiPermission01" });
            //await prepareAuthenticatedClient(new string[] { "ApiPermission01" });
            var response = await _clt.GetAsync(_baseURL);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }

        private async Task prepareAuthenticatedClient(string[] scope)
        {
            var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scope);
            Debug.WriteLine($"access token-{accessToken}");
            _clt.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }

}
