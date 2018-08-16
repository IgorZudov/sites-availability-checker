using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SitesChecker.Domain.Infrastructure
{
	public interface IMonitoringService
	{
		/// <summary>
		/// Возвращает промежуток времени между проверками
		/// </summary>
		/// <returns></returns>
		int GetTimeDelay();

		/// <summary>
		/// Обновляем состояние сайтов
		/// </summary>
		Task Update();
	}
}