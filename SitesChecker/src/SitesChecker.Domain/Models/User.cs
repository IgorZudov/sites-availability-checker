namespace SitesChecker.Domain.Models
{
	public class User
	{
		public int Id { get; set; }

		/// <summary>
		///     Логин пользователя
		/// </summary>
		public string Login { get; set; }
		/// <summary>
		///     Пароль пользователя
		/// </summary>
		public string Password { get; set; }
		/// <summary>
		///     Роль пользователя
		/// </summary>
		public string Role { get; set; }
	}
}