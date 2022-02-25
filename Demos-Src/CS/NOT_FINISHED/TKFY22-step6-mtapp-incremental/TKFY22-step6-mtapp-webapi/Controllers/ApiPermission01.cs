using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace TKFY22_step6_mtapp_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [RequiredScope("ApiPermission01")]
    public class ApiPermission01 : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("ApiPermission01 - WebApi01");
        }
    }
}
