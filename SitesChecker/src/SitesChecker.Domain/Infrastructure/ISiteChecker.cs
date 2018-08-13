using System.Collections.Generic;
using System.Threading.Tasks;
using SitesChecker.DataAccess.Models;

namespace SitesChecker.Domain.Infrastructure
{
	public interface ISiteChecker
	{
		IEnumerable<MonitoringResult> Check(IEnumerable<SiteAvailability> sites);
		Task<MonitoringResult> Check(SiteAvailability site);
	}
}