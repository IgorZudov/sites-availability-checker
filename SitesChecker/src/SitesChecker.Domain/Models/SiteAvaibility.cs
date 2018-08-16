using System;

namespace SitesChecker.Domain.Models
{
	public class SiteAvailability
	{
		public int Id { get; set; }

		public DateTimeOffset LastUpdate { get; set; }

		public bool IsAvailable { get; set; }

		public Site Site { get; set; }

		public SiteAvailability()
		{
		}

		public SiteAvailability(Site site, DateTimeOffset updateTime, bool isaVAilable)
		{
			Site = site;
			LastUpdate = updateTime;
			IsAvailable = isaVAilable;
		}
	}
}