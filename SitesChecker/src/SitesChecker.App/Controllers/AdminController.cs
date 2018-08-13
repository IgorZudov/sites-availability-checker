using System;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SitesChecker.Core;
using SitesChecker.DataAccess;
using SitesChecker.Domain.Infrastructure;

namespace SitesChecker.App.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	public class AdminController : Controller
	{
		private IMonitoringService monitoringService;
		public AdminController(IMonitoringService monitoringServ)
		{
			monitoringService = monitoringServ;
			
		}
		
		public IActionResult Index()
		{
			//todo get sites
			throw new NotImplementedException();
		}
		
		[Authorize]
		[HttpDelete("removesite")]
		public IActionResult RemoveSite()
		{
			throw new NotImplementedException();
		}
		[Authorize]
		[HttpPut("updatesite")]
		public IActionResult UpdateSite()
		{
			throw new NotImplementedException();
		}
		[Authorize]
		[HttpPost("addsite")]
		public IActionResult AddSite()
		{
			throw new NotImplementedException();
		}
		
		[Authorize]
		[HttpPut("updatedelay")]
		public IActionResult UpdateDelay(int value)
		{
			CoreConfiguration.Default.UpdateSitesDelay = value;
			CoreConfiguration.Default.Save();
			monitoringService.StopAsync(new CancellationToken());
			monitoringService.StartAsync(new CancellationToken());
			throw new NotImplementedException();
		}
		
	}
}