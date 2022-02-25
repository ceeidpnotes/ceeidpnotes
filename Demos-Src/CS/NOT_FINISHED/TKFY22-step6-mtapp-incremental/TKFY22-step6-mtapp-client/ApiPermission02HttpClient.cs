using Microsoft.Identity.Web;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;

namespace fy21_simplemtapp
{
    public class ApiPermission02HttpClient
    {
        private HttpClient _clt;

        private ITokenAcquisition _tokenAcquisition;
        private string _baseURL;

        public ApiPermission02HttpClient(ITokenAcquisition tokenAcquisition, HttpClient client, IConfiguration configuration)
        {
            _clt = client;
            _tokenAcquisition = tokenAcquisition;
            _baseURL = configuration["ApiPermission02:BaseURL"];
            _clt.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<string> CallApiNonExisting()
        {
            await prepareAuthenticatedClient(new string[] { "api://bc36543f-efc2-410b-ad6c-9826ceaceedf/ApiPermission02" });
            var response = await _clt.GetAsync(_baseURL);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }

        public async Task<string> CallApiWebApi01()
        {
            await prepareAuthenticatedClient(new string[] { "api://bc36543f-efc2-410b-ad6c-9826ceaceedf/ApiPermission02.WebApi01" });
            var response = await _clt.GetAsync(_baseURL + "/WebApi01");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }
        public async Task<string> CallApiWebApi02()
        {
            await prepareAuthenticatedClient(new string[] { "api://bc36543f-efc2-410b-ad6c-9826ceaceedf/ApiPermission02.WebApi02" });
            var response = await _clt.GetAsync(_baseURL + "/WebApi02");
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
