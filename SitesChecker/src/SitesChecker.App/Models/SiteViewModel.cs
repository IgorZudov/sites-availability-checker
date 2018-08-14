using System.ComponentModel.DataAnnotations;

namespace SitesChecker.App.Models
{
    public class SiteViewModel
    {
        public int Id { get; set; }
        [Display(Name="Название")]
        public string Name { get; set; }
        [Display(Name="Url")]
        public string Url { get; set; }
        public bool IsAvailable { get; set; }
    }
}