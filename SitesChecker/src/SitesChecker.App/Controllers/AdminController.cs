using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SitesChecker.App.Models;
using SitesChecker.App.Utils;
using SitesChecker.DataAccess;
using SitesChecker.Domain;
using SitesChecker.Domain.Infrastructure;

namespace SitesChecker.App.Controllers
{
	[Authorize]
	[Route("api/admin")]
	public class AdminController : Controller
	{
		private IMonitoringService monitoringService;
		private RequestsDelay delay;
		private IDataContext dataContext;
		public AdminController(IMonitoringService monitoringServ, IOptionsSnapshot<RequestsDelay> delayConfig, IDataContext dbContext)
		{
			monitoringService = monitoringServ;
			delay = delayConfig.Value;
			dataContext = dbContext;
		}
		
		public IActionResult Index()
		{
			var monitoringResults = monitoringService.GetResults();
			var results = monitoringResults.Select(_ => _.ToSiteViewModel()).ToList();
			return View(results);
		}
		
		[HttpDelete("removesite")]
		public IActionResult RemoveSite(SiteViewModel siteModel)
		{
			dataContext.Delete(siteModel.ToSiteAvailability());
			dataContext.CommitAsync();
			throw new NotImplementedException();
		}
		
		[HttpPut("updatesite")]
		public IActionResult UpdateSite(SiteViewModel siteModel)
		{
			dataContext.Update(siteModel.ToSiteAvailability());
			dataContext.CommitAsync();
			throw new NotImplementedException();
		}
	
		[HttpPost("addsite")]
		public IActionResult AddSite(SiteViewModel siteModel)
		{
			dataContext.Create(siteModel.ToSiteAvailability());
			dataContext.CommitAsync();
			throw new NotImplementedException();
		}
		
		[HttpPut("updatedelay")]
		public IActionResult UpdateDelay(int value)
		{
			monitoringService.UpdateTimeDelay(value);
			throw new NotImplementedException();
		}
	}
}