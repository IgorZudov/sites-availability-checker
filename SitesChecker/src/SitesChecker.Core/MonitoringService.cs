using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SitesChecker.DataAccess;
using SitesChecker.DataAccess.Models;
using SitesChecker.Domain;
using SitesChecker.Domain.Infrastructure;

namespace SitesChecker.Core
{
	public class MonitoringHostedService :  IMonitoringService
	{
		private readonly ILogger logger;
		private readonly IDataContext dataContext;
		private readonly IUrlChecker urlChecker;
		private List<MonitoringResult> lastResults;
	
		private int delay;
		public MonitoringHostedService(ILoggerFactory loggerFactory, IDataContext dbContext,IUrlChecker checker)
		{
			logger = loggerFactory.CreateLogger<MonitoringHostedService>();
			dataContext = dbContext;
			urlChecker = checker;
			delay = 5;
		}

		private void CheckResults(IEnumerable<MonitoringResult> results)
		{
			lastResults = results.ToList();
			MonitoringResultsChanged?.Invoke(lastResults);
		}
		
		private bool IsResultsExist()
		{
			return lastResults != null && lastResults.Any();
		}
		
		public IEnumerable<MonitoringResult> GetResults()
		{
			return IsResultsExist() ? lastResults : new List<MonitoringResult>();
		}

		public event Action<IEnumerable<MonitoringResult>> MonitoringResultsChanged;

		public int GetTimeDelay()
		{
			return delay;
		}

		public void UpdateTimeDelay(int timeDelay)
		{
			delay = timeDelay;
		}

		public Task Update()
		{
			return Task.Factory.StartNew(()=>
			{
				logger.LogInformation("Start checking sites");
				var sites = dataContext.Query<SiteAvailability>();
				var results = urlChecker.Check(sites.ToList());
				CheckResults(results);
			});
		}
	}
}