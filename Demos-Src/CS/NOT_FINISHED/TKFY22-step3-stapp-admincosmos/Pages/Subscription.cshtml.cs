#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using fTKFY22_step3_stapp_admincosmos;
using Microsoft.AspNetCore.Authorization;

namespace TKFY22_step3_stapp_admincosmos.Pages
{
    [Authorize(Policy = "updateTenantPolicy")]
    public class SubscriptionModel : PageModel
    {
        private readonly fTKFY22_step3_stapp_admincosmos.DatabaseContext _context;

        public SubscriptionModel(fTKFY22_step3_stapp_admincosmos.DatabaseContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Subscription Subscription { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Subscription = await _context.Subscriptions.FirstOrDefaultAsync(m => m.Id == id);

            if (Subscription == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Subscription).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubscriptionExists(Subscription.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./SubscriptionList");
        }

        private bool SubscriptionExists(string id)
        {
            return _context.Subscriptions.Any(e => e.Id == id);
        }
    }
}
