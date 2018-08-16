using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SitesChecker.DataAccess;
using SitesChecker.DataAccess.Models;

namespace SitesChecker.App.Pages
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