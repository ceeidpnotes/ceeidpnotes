using fy21_simplemtapp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace fy21_simplemtapp.Pages
{
    public class ResetModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly DatabaseContext _db;

        public ResetModel(ILogger<IndexModel> logger, fy21_simplemtapp.Model.DatabaseContext db)
        {
            _logger = logger;
            _db = db;
        }
        public async Task OnGet()
        {
            await _db.Database.EnsureDeletedAsync();
            await _db.Database.EnsureCreatedAsync();
            _db.Subscriptions.Add(new Subscription { Id = "67b8b473-b8e0-4cdf-bef6-c9b14beaba48", IsActive = true });
            _db.Subscriptions.Add(new Subscription { Id = "72f988bf-86f1-41af-91ab-2d7cd011db47", IsActive = true }); //MS
            _db.Subscriptions.Add(new Subscription { Id = "07b42849-58ba-42ca-a954-1ce79ed62182", IsActive = true });
            _db.Subscriptions.Add(new Subscription { Id = "2feaa5b1-2722-4933-afca-4d14140d5ef0", IsActive = true }); //M365x578388.onmicrosoft.com

            await _db.SaveChangesAsync();
        }
    }
}
