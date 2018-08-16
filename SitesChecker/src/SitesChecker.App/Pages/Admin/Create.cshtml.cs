using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SitesChecker.Domain.Models;

namespace SitesChecker.App.Pages.Admin
{
	public class CreateModel : PageModel
	{
		private readonly DataAccess.DataContext _context;

		[BindProperty]
		public Site Site { get; set; }

		public CreateModel(DataAccess.DataContext context)
		{
			_context = context;
		}

		public IActionResult OnGet()
		{
			return Page();
		}

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