using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SitesChecker.App.Models;
using SitesChecker.App.Utils;
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
			var results = monitoringService.GetResults().Select(_=>_.ToSiteViewModel());
			return View(results.ToList());
		}
	}
}