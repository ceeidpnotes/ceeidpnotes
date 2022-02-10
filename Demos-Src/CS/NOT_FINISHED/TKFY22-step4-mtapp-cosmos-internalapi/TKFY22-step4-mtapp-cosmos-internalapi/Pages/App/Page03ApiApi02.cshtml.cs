using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace fy21_simplemtapp.Pages.App
{
    [Authorize(Roles = "Page03Api")]
    public class Page03ApiApi02Model : PageModel
    {
        private API02HttpClient _cltapi02;
        public string Result=String.Empty;

        public Page03ApiApi02Model(API02HttpClient cltapi02)
        {
            _cltapi02 = cltapi02;
        }
        public async Task OnGet()
        {
            Result = await _cltapi02.CallApi();
        }
    }
}
