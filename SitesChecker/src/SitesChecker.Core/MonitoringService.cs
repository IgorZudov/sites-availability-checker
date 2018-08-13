using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SitesChecker.DataAccess;
using SitesChecker.Domain;
using SitesChecker.Domain.Infrastructure;

namespace SitesChecker.Core
{
	public class MonitoringHostedService : IHostedService, IDisposable, IMonitoringService
	{
		private readonly ILogger logger;
		private IDataContext dataContext;
		private Timer timer;
		private IEnumerable<MonitoringResult> lastResults;
		public MonitoringHostedService(ILoggerFactory loggerFactory, IDataContext dbContext)
		{
			logger = loggerFactory.CreateLogger<MonitoringHostedService>();
			dataContext = dbContext;
		}

		private void Monitore(object state)
		{

		}

		private bool IsResultsExist()
		{
			return lastResults != null && lastResults.Any();
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			timer = new Timer(Monitore, null, TimeSpan.Zero,
				TimeSpan.FromSeconds(5));
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			timer?.Change(Timeout.Infinite, 0);
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<MonitoringResult> GetResults()
		{
			return IsResultsExist() ? lastResults : new List<MonitoringResult>();
		}

		public event Action<IEnumerable<MonitoringResult>> MonitoringResultsChanged;
	}
}