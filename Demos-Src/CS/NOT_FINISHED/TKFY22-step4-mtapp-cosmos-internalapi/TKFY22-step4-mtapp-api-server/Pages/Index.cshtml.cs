using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Web;

namespace TKFY22_step4_mtapp_api_server.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            
            var q = HttpUtility.ParseQueryString(Request.QueryString.ToString());
            var ri = q["state"];
            if (ri!=null) Redirect(ri);
        }
    }
}
