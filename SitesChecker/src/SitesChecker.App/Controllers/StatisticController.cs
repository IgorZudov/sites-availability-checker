using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SitesChecker.App.Utils;
using SitesChecker.DataAccess;
using SitesChecker.DataAccess.Models;
using SitesChecker.Domain.Infrastructure;

namespace SitesChecker.App.Controllers
{
	[Route("api/statistic")]
	public class StatisticController : Controller
	{
		private readonly IDataContext dataContext;
		
		public StatisticController(IDataContext dbContext)
		{
			dataContext = dbContext;
		}
		
		public IActionResult Index()
		{
			var monitoringResults = dataContext.Query<SiteAvailability>().Include(_=>_.Site).ToList();
			var results = monitoringResults.Select(_=>_.ToSiteViewModel()).ToList();
			return View(results);
		}
	}
}