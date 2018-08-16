using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SitesChecker.DataAccess;
using SitesChecker.DataAccess.Models;

namespace SitesChecker.App.Pages
{
	[Authorize]
    public class IndexModel : PageModel
    {
        private readonly SitesChecker.DataAccess.DataContext _context;

        public IndexModel(SitesChecker.DataAccess.DataContext context)
        {
            _context = context;
        }

        public IList<Site> Site { get;set; }

        public async Task OnGetAsync()
        {
            Site = await _context.Sites.ToListAsync();
        }
    }
}
