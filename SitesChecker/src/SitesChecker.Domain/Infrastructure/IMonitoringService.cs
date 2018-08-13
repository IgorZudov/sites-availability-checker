using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace SitesChecker.Domain.Infrastructure
{
	public interface IMonitoringService
	{
		/// <summary>
		/// Возвращает результаты мониторинга, в случае их отсуствия возвращается пустая коллекция
		/// </summary>
		/// <returns></returns>
		IEnumerable<MonitoringResult> GetResults();
		/// <summary>
		/// Событие изменения результатов мониторинга
		/// </summary>
		event Action<IEnumerable<MonitoringResult>> MonitoringResultsChanged;
	}
}