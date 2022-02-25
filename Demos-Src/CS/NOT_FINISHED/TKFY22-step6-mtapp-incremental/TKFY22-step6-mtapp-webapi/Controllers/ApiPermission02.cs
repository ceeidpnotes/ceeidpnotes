using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace TKFY22_step6_mtapp_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApiPermission02 : ControllerBase
    {
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
        public IActionResult WebApi02()
        {
            return Ok("ApiPermission02 - WebApi02");
        }
    }
}
