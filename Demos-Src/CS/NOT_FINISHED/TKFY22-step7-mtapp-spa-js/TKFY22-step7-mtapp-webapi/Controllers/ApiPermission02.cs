using fy21_simplemtapp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;

namespace TKFY22_step6_mtapp_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApiPermission02 : ControllerBase
    {
        private ITokenAcquisition _tokenAcquisition;

        public ApiPermission02(ITokenAcquisition tokenAcquisition)
        {
            _tokenAcquisition = tokenAcquisition;
        }
        [HttpGet]
        [RequiredScope("ApiPermission02.WebApi01")]
        [Route("WebApi01")]
        public IActionResult WebApi01()
        {
            return Ok("ApiPermission02 - WebApi01");
        }

        [HttpGet]
        [RequiredScope("ApiPermission02.WebApi02")]
        [Route("WebApi02")]
        public async Task<IActionResult> WebApi02()
        {
            var tenant = User.Claims.FirstOrDefault(x => x.Type is "tid" or "http://schemas.microsoft.com/identity/claims/tenantid")?.Value;
            var upn = User.Claims.FirstOrDefault(x => x.Type is "upn" or "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn")?.Value;

            GraphServiceClient graphClient = getGraphServiceClient(new[] { "User.Read" });

            var Me = await graphClient.Me.Request().GetAsync();

            return Ok($"ApiPermission02 - WebApi02,ok - {tenant} - {upn}; " +
                      $"GraphAPI, preffered language: {Me?.PreferredLanguage}, Additional {Me?.AdditionalData.Count}; "
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
