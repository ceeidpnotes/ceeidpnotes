#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using fTKFY22_step3_stapp_admincosmos;
using Microsoft.AspNetCore.Authorization;

namespace TKFY22_step3_stapp_admincosmos.Pages
{
    [Authorize(Roles = "listTenants")]
    public class SubscriptionListModel : PageModel
    {
        private readonly fTKFY22_step3_stapp_admincosmos.DatabaseContext _context;

        public SubscriptionListModel(fTKFY22_step3_stapp_admincosmos.DatabaseContext context)
        {
            _context = context;
        }

        public IList<Subscription> Subscription { get;set; }

        public async Task OnGetAsync()
        {
            Subscription = await _context.Subscriptions.ToListAsync();
        }
    }
}
