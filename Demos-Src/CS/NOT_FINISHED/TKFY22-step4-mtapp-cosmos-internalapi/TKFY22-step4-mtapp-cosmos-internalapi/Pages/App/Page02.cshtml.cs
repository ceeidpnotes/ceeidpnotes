using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace fy21_simplemtapp.Pages.App
{
    [Authorize(Roles = "Page02")]
    public class Page02Model : PageModel
    {
        public void OnGet()
        {
        }
    }
}
