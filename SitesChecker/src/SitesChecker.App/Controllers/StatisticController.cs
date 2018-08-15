using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SitesChecker.App.Utils;
using SitesChecker.Domain.Infrastructure;

namespace SitesChecker.App.Controllers
{
	[Route("api/[controller]")]
	public class StatisticController : Controller
	{
		private readonly IMonitoringService monitoringService;
		
		public StatisticController(IMonitoringService monitoringServ)
		{
			monitoringService = monitoringServ;
		}
		
		public IActionResult Index()
		{
			var monitoringResults = monitoringService.GetResults();
			var results = monitoringResults.Select(_=>_.ToSiteViewModel()).ToList();
			return View(results);
		}
	}
}