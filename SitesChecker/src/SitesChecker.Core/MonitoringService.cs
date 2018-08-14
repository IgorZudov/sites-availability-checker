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
		private readonly IDataContext dataContext;
		private readonly IUrlChecker urlChecker;
		private Timer timer;
		private List<MonitoringResult> lastResults;
		private readonly IMonitoringResultsComparer resultsComparer;

		public MonitoringHostedService(ILoggerFactory loggerFactory, IDataContext dbContext,IUrlChecker checker, IMonitoringResultsComparer comparer )
		{
			logger = loggerFactory.CreateLogger<MonitoringHostedService>();
			dataContext = dbContext;
			urlChecker = checker;
			resultsComparer = comparer;
		}

		private void CheckResults(IEnumerable<MonitoringResult> results)
		{
			if (lastResults == null)
			{
				lastResults = results.ToList();
				MonitoringResultsChanged?.Invoke(lastResults);
			}

			var deletedResults = resultsComparer.GetDeletedResults(lastResults,results);
			var toBeUpdatedResults = resultsComparer.GetUpdatedResults(lastResults, results);
			var newResults = resultsComparer.GetNewResults(lastResults, results);
			var isResultsChanged=resultsComparer.SetResults(lastResults,deletedResults,toBeUpdatedResults,newResults);
			if(isResultsChanged) MonitoringResultsChanged?.Invoke(lastResults);
		}
		
		private void Monitore(object state)
		{
			logger.LogInformation("Start checking sites");
			var sites = dataContext.Query<SiteAvailability>();
			var results = urlChecker.Check(sites.ToList());
			CheckResults(results);
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
			Monitore(new object());
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