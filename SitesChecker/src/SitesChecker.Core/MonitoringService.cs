using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SitesChecker.DataAccess;
using SitesChecker.Domain.Infrastructure;
using SitesChecker.Domain.Models;

namespace SitesChecker.Core
{
	public class MonitoringHostedService : IMonitoringService
	{
		private readonly ILogger logger;
		private readonly IDataContext dataContext;
		private readonly IUrlChecker urlChecker;

		public MonitoringHostedService(ILoggerFactory loggerFactory, IDataContext dbContext, IUrlChecker checker)
		{
			logger = loggerFactory.CreateLogger<MonitoringHostedService>();
			dataContext = dbContext;
			urlChecker = checker;
		}

		public int GetTimeDelay()
		{
			var sites = dataContext.Query<Site>();
			return !sites.Any() ? 0 : sites.Select(_ => _.UpdateDelay).Min();
		}

		private bool CanPartialUpdate(ICollection availabilities, ICollection sites)
		{
			return availabilities.Count > 0 && availabilities.Count == sites.Count;
		}

		public void UpdateAvaliability(IEnumerable<SiteAvailability> results)
		{
			foreach (var result in results)
			{
				dataContext.Delete(new SiteAvailability()
				{
					LastUpdate = result.LastUpdate,
					Site = result.Site

				});
				dataContext.CommitAsync();
			}

			foreach (var availability in results)
			{
				dataContext.Create(availability);
			}
			dataContext.CommitAsync();
		}
		private List<Site> GetNeedUpdateSites(IEnumerable<SiteAvailability> availabilities)
		{
			return (from availability in availabilities where (DateTimeOffset.Now - availability.LastUpdate).TotalSeconds >= availability.Site.UpdateDelay select availability.Site).ToList();
		}
		public Task Update()
		{
			return Task.Factory.StartNew(() =>
			{
				logger.LogInformation("Start checking sites");
				var avaliabilities = dataContext.Query<SiteAvailability>().Include(_ => _.Site).ToList();
				var sites=dataContext.Query<Site>().ToList();
				var needCheck = CanPartialUpdate(avaliabilities,sites) ? GetNeedUpdateSites(avaliabilities) : sites;
				var results = urlChecker.Check(needCheck);
				UpdateAvaliability(results);
			});
		}
	}
}