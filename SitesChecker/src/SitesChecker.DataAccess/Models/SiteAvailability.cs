namespace SitesChecker.DataAccess.Models
{
	public class SiteAvailability
	{
		public int Id { get; set; }
		/// <summary>
		/// Название сайта
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// URL сайта
		/// </summary>
		public  string Url { get; set; }
	
	}
}