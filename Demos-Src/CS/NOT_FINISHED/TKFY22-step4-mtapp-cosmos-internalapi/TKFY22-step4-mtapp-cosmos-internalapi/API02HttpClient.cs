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
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                HandleChallengeFromWebApi(response);
            }

            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }

        private async Task prepareAuthenticatedClient()
        {
            var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { _scope });
            Debug.WriteLine($"access token-{accessToken}");
            _clt.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        private static string? GetParameter(IEnumerable<string> parameters, string parameterName)
        {
            int offset = parameterName.Length + 1;
            return parameters?.FirstOrDefault(p => p.StartsWith($"{parameterName}="))?.Substring(offset)?.Trim('"');
        }
        private void HandleChallengeFromWebApi(HttpResponseMessage response)
        {
            //proposedAction="consent"
            List<string> result = new List<string>();
            AuthenticationHeaderValue bearer = response.Headers.WwwAuthenticate.First(v => v.Scheme == "Bearer");
            IEnumerable<string> parameters = bearer?.Parameter.Split(',').Select(v => v.Trim()).ToList();
            string? proposedAction = GetParameter(parameters, "proposedAction");

            if (proposedAction == "consent")
            {
                string consentUri = GetParameter(parameters, "consentUri");

                var uri = new Uri(consentUri);

                //Set values of query string parameters
                var queryString = System.Web.HttpUtility.ParseQueryString(uri.Query);
                queryString.Set("redirect_uri", _apiRedirectUri);
                queryString.Add("prompt", "consent");
                queryString.Add("state", _redirectUri);
                //Update values in consent Uri
                var uriBuilder = new UriBuilder(uri);
                uriBuilder.Query = queryString.ToString();
                var updateConsentUri = uriBuilder.Uri.ToString();
                result.Add("consentUri");
                result.Add(updateConsentUri);

                //throw custom exception
                throw new WebApiMsalUiRequiredException(updateConsentUri);
            }
        }
    }

}
