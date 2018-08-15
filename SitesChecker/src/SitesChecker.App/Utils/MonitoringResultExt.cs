using SitesChecker.App.Models;
using SitesChecker.DataAccess.Models;
using SitesChecker.Domain;

namespace SitesChecker.App.Utils
 {
     public static class MonitoringResultExt
     {
         //todo need configure automapper
         public static SiteViewModel ToSiteViewModel(this MonitoringResult result)
         {
             return new SiteViewModel()
             {
                 Id = result.SiteInfo.Id,
                 Name = result.SiteInfo.Name,
                 Url = result.SiteInfo.Url,
                 IsAvailable = result.IsAvailable,
             };
         }
	     public static SiteAvailability ToSiteAvailability(this SiteViewModel result)
	     {
		     return new SiteAvailability()
		     {
			     Id = result.Id,
			     Name = result.Name,
			     Url = result.Url
		     };
	     }
	}
 }