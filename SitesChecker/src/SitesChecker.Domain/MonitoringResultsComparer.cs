using System.Collections.Generic;
using System.Linq;
using SitesChecker.Domain.Infrastructure;

namespace SitesChecker.Domain
{
	public class MonitoringResultsComparer:IMonitoringResultsComparer
	{
		public List<MonitoringResult> GetDeletedResults(IEnumerable<MonitoringResult> masterResults, IEnumerable<MonitoringResult> newResults)
		{
			return masterResults.Where(c => newResults.All(d => c.SiteInfo.Url != d.SiteInfo.Url)).ToList();
		}
		public List<MonitoringResult> GetUpdatedResults(IEnumerable<MonitoringResult> masterResults, IEnumerable<MonitoringResult> newResults)
		{
			return masterResults.Where(c => newResults.All(d => c.SiteInfo.Url != d.SiteInfo.Url)).ToList();
		}
		public List<MonitoringResult> GetNewResults(IEnumerable<MonitoringResult> masterResults, IEnumerable<MonitoringResult> newResults)
		{
			return masterResults.Where(c => newResults.All(d => c.SiteInfo.Url != d.SiteInfo.Url)).ToList();
		}

		public bool SetResults(List<MonitoringResult> masterResults, IReadOnlyCollection<MonitoringResult> deletedResults, IReadOnlyCollection<MonitoringResult> toBeUpdatedResults,
			IReadOnlyCollection<MonitoringResult> newResults)
		{
			bool result = deletedResults.Any()|| toBeUpdatedResults.Any()||newResults.Any();
			foreach (var deleteResult in deletedResults)
			{
				masterResults.Remove(deleteResult);
			}
			masterResults.AddRange(newResults);
			foreach (var lastResult in masterResults)
			{
				foreach (var updatedResult in toBeUpdatedResults)
				{
					if (lastResult.SiteInfo.Url == updatedResult.SiteInfo.Url)
					{
						lastResult.IsAvailable = updatedResult.IsAvailable;
					}
				}
			}
			return result;
		}
	}
}