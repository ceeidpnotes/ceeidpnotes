using fy21_simplemtapp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;

namespace TKFY22_step6_mtapp_client.Pages.Call
{
    /*
     * Important - try to remove Authorize
     * And try to change order - first AuthorizeForScope, then Authorize
     * (and see set of beautifull errors!)
     */

    [Authorize]
    [AuthorizeForScopes(Scopes = new[] { "User.Read", "api://bc36543f-efc2-410b-ad6c-9826ceaceedf/ApiPermission01" })]
    public class CallApiPermission01Model : PageModel
    {
        private ApiPermission01HttpClient _clt;
        public string Result;

        public CallApiPermission01Model(ApiPermission01HttpClient clt)
        {
            _clt = clt;
            Result = String.Empty;
        }
        public async Task OnGet()
        {
            Result = await _clt.CallApi();
        }
    }
}
