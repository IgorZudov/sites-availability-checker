using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SitesChecker.DataAccess.Models;
using SitesChecker.Domain.Infrastructure;

namespace SitesChecker.Domain
{
	public class SiteChecker:ISiteChecker
	{
		private ILogger logger;
		public SiteChecker(ILoggerFactory loggerFactory)
		{
			logger = loggerFactory.CreateLogger<SiteChecker>();
		}
		public IEnumerable<MonitoringResult> Check(IEnumerable<SiteAvailability> sites)
		{
			var result=new List<MonitoringResult>();
			foreach (var site in sites)
			{
				result.Add(Check(site).Result);
			}
			return result;
		}

		public async Task<MonitoringResult> Check(SiteAvailability site)
		{
			return await Task.Factory.StartNew(() =>
			{
				var request = WebRequest.Create($"{site.Url}");
				var response = (HttpWebResponse) (request.GetResponseAsync().Result);
				if (response == null || response.StatusCode != HttpStatusCode.OK) return new MonitoringResult(site, true);
				return new MonitoringResult(site, false);
			});
		}
	}
}