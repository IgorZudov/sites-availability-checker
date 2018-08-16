using SitesChecker.App.Models;
using SitesChecker.Domain;
using SitesChecker.Domain.Models;

namespace SitesChecker.App.Utils
 {
     public static class MonitoringResultExt
     {
         //todo need configure automapper
         public static SiteViewModel ToSiteViewModel(this SiteAvailability result)
         {
             return new SiteViewModel()
             {
                 Id = result.Id,
                 Name = result.Site.Name,
                 Url = result.Site.Url,
                 IsAvailable = result.IsAvailable,
				 LastUpdateTime = result.LastUpdate
             };
         }
	}
 }