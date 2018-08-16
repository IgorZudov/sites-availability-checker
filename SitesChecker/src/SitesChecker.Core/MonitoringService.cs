using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SitesChecker.DataAccess;
using SitesChecker.Domain;
using SitesChecker.Domain.Infrastructure;
using SitesChecker.Domain.Models;

namespace SitesChecker.Core
{
	public class MonitoringHostedService :  IMonitoringService
	{
		private readonly ILogger logger;
		private readonly IDataContext dataContext;
		private readonly IUrlChecker urlChecker;
	
		public MonitoringHostedService(ILoggerFactory loggerFactory, IDataContext dbContext,IUrlChecker checker)
		{
			logger = loggerFactory.CreateLogger<MonitoringHostedService>();
			dataContext = dbContext;
			urlChecker = checker;
		}
		public int GetTimeDelay()
		{
			var sites = dataContext.Query<Site>();
			//todo рассчитывать те сайты, которые необходимо обновлять в рамках текущего Update, через сущность UpdateAvailability
			return !sites.Any() ? 0 : sites.Select(_ => _.UpdateDelay).Min();
		}
		
		public Task Update()
		{
			return Task.Factory.StartNew(()=>
			{
				logger.LogInformation("Start checking sites");
				var sites = dataContext.Query<Site>().ToList();
				var results = urlChecker.Check(sites);
				dataContext.DeleteRange(dataContext.Query<SiteAvailability>().ToList());
				dataContext.CommitAsync();
				foreach (var availability in results)
				{
					dataContext.Create(availability);
				}
				dataContext.CommitAsync();
			});
		}
	}
}