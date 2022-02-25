using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace fy21_simplemtapp.Pages
{
    [Authorize]
    public class LoginMTModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
