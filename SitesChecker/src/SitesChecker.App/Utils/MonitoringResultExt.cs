using SitesChecker.App.Models;
using SitesChecker.DataAccess.Models;
using SitesChecker.Domain;

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