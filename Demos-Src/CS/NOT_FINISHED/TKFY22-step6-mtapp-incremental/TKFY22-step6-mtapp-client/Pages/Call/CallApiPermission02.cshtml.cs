using fy21_simplemtapp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;

namespace TKFY22_step6_mtapp_client.Pages.Call
{
    [Authorize]
    [AuthorizeForScopes(Scopes = new[] { "User.Read", "api://bc36543f-efc2-410b-ad6c-9826ceaceedf/ApiPermission02" })]
    public class CallApiPermission02Model : PageModel
    {
        private ApiPermission02HttpClient _clt;
        public string Result;

        public CallApiPermission02Model(ApiPermission02HttpClient clt)
        {
            _clt = clt;
            Result = String.Empty;
        }
        //WE DON'T HAVE ApiPermission02 only - but - we can requests (and see error)
        public async Task OnGet()
        {
            Result = await _clt.CallApiNonExisting();
        }
    }
}
