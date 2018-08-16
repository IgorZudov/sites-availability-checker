using System.Collections.Generic;
using SitesChecker.DataAccess.Models;

namespace SitesChecker.Domain.Infrastructure
{
	public interface IUrlChecker
	{
		IEnumerable<SiteAvailability> Check(List<Site> sites);
	
	}
}