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
		
		public static SiteAvailability NotAvailabile(Site site, DateTimeOffset updateTime)
		{
			return new SiteAvailability
			{
				Site = site,
				LastUpdate = updateTime,
				IsAvailable = false
			};
		}
		
		public static SiteAvailability Availabile(Site site, DateTimeOffset updateTime)
		{
			return new SiteAvailability
			{
				Site = site,
				LastUpdate = updateTime,
				IsAvailable = true
			};
		}
	}
}