using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SitesChecker.DataAccess.Models;
using SitesChecker.Domain.Infrastructure;
using SitesChecker.Domain.Utils;

namespace SitesChecker.Domain
{
	public class UrlChecker : IUrlChecker
	{
		private readonly ILogger logger;
		private readonly IResponseDataProvider responseDataProvider;
		public UrlChecker(ILoggerFactory loggerFactory, IResponseDataProvider responseProvider)
		{
			logger = loggerFactory.CreateLogger<UrlChecker>();
			responseDataProvider = responseProvider;
		}

		private async Task<List<SiteAvailability>> Run(ICollection<Task<SiteAvailability>> tasks)
		{
			var result = new List<SiteAvailability>();
			while (tasks.Count > 0)
			{
				var firstFinishedTask = await Task.WhenAny(tasks);
				tasks.Remove(firstFinishedTask);
				result.Add(await firstFinishedTask);
			}
			return result;
		}
		public IEnumerable<SiteAvailability> Check(List<Site> sites)
		{
			if (sites == null) throw new ArgumentNullException();
			logger.LogInformation($"{sites.Count()}-site availability check request");
			var tasks=new List<Task<SiteAvailability>>();
			foreach (var site in sites)
			{
				tasks.Add(CheckAsync(site));
			}
			return Run(tasks).Result;
		}

		private async Task<SiteAvailability> CheckAsync(Site site)
		{
			if (site == null) throw new ArgumentNullException();
			if (!UrlHelper.IsUrlValid(site.Url))
			{
				logger.LogWarning($"The {site.Url} is not valid URL");
				return new SiteAvailability(site,DateTimeOffset.Now, false);
			}

			return await Task.Factory.StartNew(() =>
			{
				try
				{
					if (responseDataProvider.IsResponseAvailable(site.Url))
					{
						logger.LogInformation($"The {site.Url} is avaiable");
						return new SiteAvailability(site, DateTimeOffset.Now, true);
					}
				}
				catch (UriFormatException e)
				{
					logger.LogError($"The {site.Url} is not valid");
					return new SiteAvailability(site, DateTimeOffset.Now, false);
				}
				catch (Exception e)
				{
					return new SiteAvailability(site, DateTimeOffset.Now, false);
				}

				logger.LogWarning($"The site {site.Url} is unavailable");
				return new SiteAvailability(site, DateTimeOffset.Now, false);
			});
		}

		
	}
}