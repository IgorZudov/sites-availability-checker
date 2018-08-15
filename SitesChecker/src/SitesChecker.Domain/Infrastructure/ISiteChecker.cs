using System.Collections.Generic;
using System.Threading.Tasks;
using SitesChecker.DataAccess.Models;

namespace SitesChecker.Domain.Infrastructure
{
	public interface IUrlChecker
	{
		IEnumerable<MonitoringResult> Check(List<SiteAvailability> sites);
	
	}
}