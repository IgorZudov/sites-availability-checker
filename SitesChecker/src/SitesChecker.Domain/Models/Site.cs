namespace SitesChecker.Domain.Models
{
	public class Site
	{
		public int Id { get; set; }

		/// <summary>
		///     Название сайта
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		///     URL сайта
		/// </summary>
		public string Url { get; set; }
		/// <summary>
		///     Время обновления доступности
		/// </summary>
		public int UpdateDelay { get; set; }

		public SiteAvailability Availability { get; set; }
	}
}