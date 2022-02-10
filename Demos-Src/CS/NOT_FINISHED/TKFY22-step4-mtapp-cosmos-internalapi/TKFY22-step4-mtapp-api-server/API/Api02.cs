using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace fy21_simplemtapp.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class Api02 : ControllerBase
    {
        public Api02()
        {
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Api02 - working,ok");
        }
    }
}
