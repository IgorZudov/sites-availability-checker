using System;
using Microsoft.AspNetCore.Mvc;
using SitesChecker.Domain.Infrastructure;

namespace SitesChecker.App.Controllers
{
	[Route("api/[controller]")]
	public class StatisticController : Controller
	{
		private IMonitoringService monitoringService;
		
		public StatisticController(IMonitoringService monitoringServ)
		{
			monitoringService = monitoringServ;
		}
		
		public IActionResult Index()
		{
			throw new NotImplementedException();
			//return View(monitoringService.GetResults());
		}
	}
}