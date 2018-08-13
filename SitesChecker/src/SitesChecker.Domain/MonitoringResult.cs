using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SitesChecker.DataAccess.Models;

namespace SitesChecker.Domain
{
	public class MonitoringResult
	{
		public SiteAvailability SiteInfo { get; }
		public bool IsAvailable { get;  }

		public MonitoringResult(SiteAvailability site, bool result)
		{
			SiteInfo = site;
			IsAvailable = result;
		}
	}
}