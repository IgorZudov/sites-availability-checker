using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SitesChecker.Domain.Models;

namespace SitesChecker.App.Pages.Admin
{
	[Authorize]
	public class IndexModel : PageModel
	{
		private readonly DataAccess.DataContext _context;

		public IList<Site> Site { get; set; }

		public IndexModel(DataAccess.DataContext context)
		{
			_context = context;
		}
		
		public async Task OnGetAsync()
		{
			Site = await _context.Sites.ToListAsync();
		}
	}
}