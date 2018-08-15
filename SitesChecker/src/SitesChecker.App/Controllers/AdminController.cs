using System;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SitesChecker.App.Utils;
using SitesChecker.Core;
using SitesChecker.DataAccess;
using SitesChecker.Domain;
using SitesChecker.Domain.Infrastructure;

namespace SitesChecker.App.Controllers
{
	//[Authorize]
	[Route("api/admin")]
	public class AdminController : Controller
	{
		private IMonitoringService monitoringService;
		private RequestsDelay delay;
		public AdminController(IMonitoringService monitoringServ, IOptionsSnapshot<RequestsDelay> delayConfig)
		{
			monitoringService = monitoringServ;
			delay = delayConfig.Value;
		}
		
		public IActionResult Index()
		{
			var monitoringResults = monitoringService.GetResults();
			var results = monitoringResults.Select(_ => _.ToSiteViewModel()).ToList();
			return View(results);
		}
		
		
		[HttpDelete("removesite")]
		public IActionResult RemoveSite()
		{
			throw new NotImplementedException();
		}
		
		[HttpPut("updatesite")]
		public IActionResult UpdateSite()
		{
			throw new NotImplementedException();
		}
	
		[HttpPost("addsite")]
		public IActionResult AddSite()
		{
			throw new NotImplementedException();
		}
		
		[HttpPut("updatedelay")]
		public IActionResult UpdateDelay(int value)
		{
			delay.Delay = value;
			throw new NotImplementedException();
		}
	}
}