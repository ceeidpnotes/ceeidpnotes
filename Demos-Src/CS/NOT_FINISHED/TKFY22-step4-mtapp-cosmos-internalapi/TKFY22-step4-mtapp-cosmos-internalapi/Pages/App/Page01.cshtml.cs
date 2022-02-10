using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace fy21_simplemtapp.Pages.App
{
    [Authorize(Policy = "Page01policy")]
    public class Page01Model : PageModel
    {
        public void OnGet()
        {
        }
    }
}
