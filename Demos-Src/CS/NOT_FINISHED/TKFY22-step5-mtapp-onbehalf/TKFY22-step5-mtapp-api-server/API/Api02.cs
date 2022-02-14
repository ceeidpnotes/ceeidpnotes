using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Microsoft.Graph;
using Microsoft.Identity.Web;

namespace fy21_simplemtapp.API
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize] //Do not care about scope etc.
    [Authorize]
    public class Api02 : ControllerBase
    {
        private ITokenAcquisition _tokenAcquisition;
        private API01HttpClient _clt;

        public Api02(ITokenAcquisition tokenAcquisition, API01HttpClient clt)
        {
            _tokenAcquisition = tokenAcquisition;
            _clt = clt;
        }

        public User Me { get; private set; }

        [HttpGet] //not working with   api://a3fb2c72-23c8-4790-ae48-20da660ff2eb/Api02
        [RequiredScope("Api02")] //HttpResponseMessage: Forbidden if not on scope
        public async Task<IActionResult> Get()
        {
            //By default, without "access_as_user"
            var tenant = User.Claims.FirstOrDefault(x => x.Type is "tid" or "http://schemas.microsoft.com/identity/claims/tenantid")?.Value;
            var upn = User.Claims.FirstOrDefault(x => x.Type is "upn" or "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn")?.Value;

            GraphServiceClient graphClient = getGraphServiceClient(new[] { "User.Read" });

            Me = await graphClient.Me.Request().GetAsync();

            var r1 = await _clt.CallApi();

            return Ok($"Api02 - working,ok - {tenant} - {upn}; "+
                      $"GraphAPI, preffered language: {Me?.PreferredLanguage}, Additional {Me?.AdditionalData.Count}; " +
                      $"Call Api01 {r1}"
                      );
        }
        private GraphServiceClient getGraphServiceClient(string[] scopes)
        {
            return GraphServiceClientFactory.GetAuthenticatedGraphClient(async () =>
            {
                string result = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);
                return result;
            }, "https://graph.microsoft.com/v1.0");
        }

    }
}
