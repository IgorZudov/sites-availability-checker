using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SitesChecker.Domain.Models;

namespace SitesChecker.App.Pages.Admin
{
	
    public class CreateModel : PageModel
    {
        private readonly SitesChecker.DataAccess.DataContext _context;

        public CreateModel(SitesChecker.DataAccess.DataContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Site Site { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Sites.Add(Site);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}