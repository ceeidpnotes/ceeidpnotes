using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace fy21_simplemtapp.Pages.App
{
    //[Authorize(Roles = "Page03Api")]
    [Authorize()]
    public class Page03ApiApi01Model : PageModel
    {
        public string Result=String.Empty;
        private API01HttpClient _clt;

        public Page03ApiApi01Model(API01HttpClient clt)
        {
            _clt = clt;
        }
        public async Task OnGet()
        {
            Result = await _clt.CallApi();
        }
    }
}
