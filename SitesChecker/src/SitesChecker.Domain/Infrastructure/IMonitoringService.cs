using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace SitesChecker.Domain.Infrastructure
{
	public interface IMonitoringService:IHostedService
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