using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SitesChecker.Domain.Models;

namespace SitesChecker.App.Pages.Admin
{
	public class DeleteModel : PageModel
	{
		private readonly DataAccess.DataContext _context;

		[BindProperty]
		public Site Site { get; set; }

		public DeleteModel(DataAccess.DataContext context)
		{
			_context = context;
		}

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

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Site = await _context.Sites.FindAsync(id);
			if (Site != null)
			{
				_context.Sites.Remove(Site);
				await _context.SaveChangesAsync();
			}

			return RedirectToPage("./Index");
		}
	}
}