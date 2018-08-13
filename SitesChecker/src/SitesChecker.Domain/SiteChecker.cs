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
	public class SiteChecker:ISiteChecker
	{
		private readonly ILogger logger;
		public SiteChecker(ILoggerFactory loggerFactory)
		{
			logger = loggerFactory.CreateLogger<SiteChecker>();
		}
		public IEnumerable<MonitoringResult> Check(List<SiteAvailability> sites)
		{
			if (sites == null) throw new ArgumentNullException();
			logger.LogInformation($"{sites.Count()}-site availability check request");
			return sites.Select(site => Check(site).Result).ToList();
		}

		private bool IsResponceAvailable(HttpWebResponse response)
		{
			return !(response == null || response.StatusCode != HttpStatusCode.OK);
		}
		public async Task<MonitoringResult> Check(SiteAvailability site)
		{
			if (site==null) throw new ArgumentNullException();
			if (!UrlHelper.IsUrlValid(site.Url))
			{
				logger.LogWarning($"The {site.Url} is not valid URL");
				return new MonitoringResult(site,false);
			}
			return await Task.Factory.StartNew(() =>
			{
				var request = WebRequest.Create($"{site.Url}");
				var response = (HttpWebResponse) (request.GetResponseAsync().Result);
				if (IsResponceAvailable(response))
				{
					logger.LogInformation($"The {site.Url} is avaiable");
					return new MonitoringResult(site, true);
				}
				logger.LogWarning($"The site {site.Url} is unavailable");
				return new MonitoringResult(site, false);
				
			});
		}
	}
}