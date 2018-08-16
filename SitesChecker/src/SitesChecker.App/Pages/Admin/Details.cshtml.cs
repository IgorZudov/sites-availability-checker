using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SitesChecker.Domain.Models;

namespace SitesChecker.App.Pages.Admin
{
    public class DetailsModel : PageModel
    {
        private readonly SitesChecker.DataAccess.DataContext _context;

        public DetailsModel(SitesChecker.DataAccess.DataContext context)
        {
            _context = context;
        }

        public Site Site { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Site = await _context.Sites.SingleOrDefaultAsync(m => m.Id == id);

            if (Site == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
