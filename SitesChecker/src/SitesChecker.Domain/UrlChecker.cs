﻿using System;
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

		private async Task<List<MonitoringResult>> Run(ICollection<Task<MonitoringResult>> tasks)
		{
			var result = new List<MonitoringResult>();
			while (tasks.Count > 0)
			{
				var firstFinishedTask = await Task.WhenAny(tasks);
				tasks.Remove(firstFinishedTask);
				result.Add(await firstFinishedTask);
			}
			return result;
		}
		public IEnumerable<MonitoringResult> Check(List<SiteAvailability> sites)
		{
			if (sites == null) throw new ArgumentNullException();
			logger.LogInformation($"{sites.Count()}-site availability check request");
			var tasks=new List<Task<MonitoringResult>>();
			foreach (var site in sites)
			{
				tasks.Add(CheckAsync(site));
			}
			return Run(tasks).Result;
		}

		private async Task<MonitoringResult> CheckAsync(SiteAvailability site)
		{
			if (site == null) throw new ArgumentNullException();
			if (!UrlHelper.IsUrlValid(site.Url))
			{
				logger.LogWarning($"The {site.Url} is not valid URL");
				return new MonitoringResult(site, false);
			}

			return await Task.Factory.StartNew(() =>
			{
				try
				{
					var request = WebRequest.Create($"{site.Url}");
					var response = (HttpWebResponse) (request.GetResponseAsync().Result);
					if (responseDataProvider.IsResponseAvailable(response))
					{
						logger.LogInformation($"The {site.Url} is avaiable");
						return new MonitoringResult(site, true);
					}
				}
				catch (UriFormatException e)
				{
					logger.LogError($"The {site.Url} is not valid");
					return new MonitoringResult(site, false);
				}
				catch (Exception e)
				{
					return new MonitoringResult(site, false);
				}

				logger.LogWarning($"The site {site.Url} is unavailable");
				return new MonitoringResult(site, false);
			});
		}

		
	}
}