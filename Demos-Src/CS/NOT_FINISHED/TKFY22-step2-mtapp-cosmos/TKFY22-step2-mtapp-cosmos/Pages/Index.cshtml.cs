using fy21_simplemtapp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace fy21_simplemtapp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly DatabaseContext _db;

        public IndexModel(ILogger<IndexModel> logger, fy21_simplemtapp.Model.DatabaseContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task OnGet()
        {

        }
    }
}