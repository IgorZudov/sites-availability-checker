using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SitesChecker.DataAccess;
using SitesChecker.DataAccess.Models;
using SitesChecker.Domain;
using SitesChecker.Domain.Infrastructure;

namespace SitesChecker.Core
{
	public class MonitoringHostedService :  IDisposable, IMonitoringService
	{
		private readonly ILogger logger;
		private IDataContext dataContext;
		private ISiteChecker siteChecker;
		private Timer timer;
		private IEnumerable<MonitoringResult> lastResults;
		
		public MonitoringHostedService(ILoggerFactory loggerFactory, IDataContext dbContext,ISiteChecker checker)
		{
			logger = loggerFactory.CreateLogger<MonitoringHostedService>();
			dataContext = dbContext;
			siteChecker = checker;
		}

		private void CheckResults(IEnumerable<MonitoringResult> results)
		{
			//todo checking changes and invoke event
		}
		
		private void Monitore(object state)
		{
			logger.LogInformation("Start checking sites");
			var sites = dataContext.Query<SiteAvailability>();
			var result = siteChecker.Check(sites.ToList());
		}

		private bool IsResultsExist()
		{
			return lastResults != null && lastResults.Any();
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			var updateDelay = CoreConfiguration.Default.UpdateSitesDelay;
			timer = new Timer(Monitore, null, TimeSpan.Zero,
				TimeSpan.FromSeconds(updateDelay));
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			timer?.Change(Timeout.Infinite, 0);
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			timer?.Change(Timeout.Infinite, 0);
		}

		public IEnumerable<MonitoringResult> GetResults()
		{
			return IsResultsExist() ? lastResults : new List<MonitoringResult>();
		}

		public event Action<IEnumerable<MonitoringResult>> MonitoringResultsChanged;
	}
}