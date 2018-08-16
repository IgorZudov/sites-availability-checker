using System;
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
		[Display(Name = "Доступность")]
        public bool IsAvailable { get; set; }
	    [Display(Name = "Время последнего обновления")]
		public DateTimeOffset LastUpdateTime { get; set; }
    }
}