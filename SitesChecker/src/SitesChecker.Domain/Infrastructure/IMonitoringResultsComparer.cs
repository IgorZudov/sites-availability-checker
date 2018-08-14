using System.Collections.Generic;

namespace SitesChecker.Domain.Infrastructure
{
	/// <summary>
	/// Предназначен для сопоставления полученных результатов с текущими
	/// </summary>
	public interface IMonitoringResultsComparer
	{
		/// <summary>
		/// Получаем список результатов, которые больше не требуются
		/// </summary>
		List<MonitoringResult> GetDeletedResults(IEnumerable<MonitoringResult> masterResults,
			IEnumerable<MonitoringResult> newResults);
		/// <summary>
		/// Получаем список результатов, которые были обновлены
		/// </summary>
		List<MonitoringResult> GetUpdatedResults(IEnumerable<MonitoringResult> masterResults,
			IEnumerable<MonitoringResult> newResults);

		/// <summary>
		/// Получаем список результатов, которые были добавлены
		/// </summary>
		List<MonitoringResult> GetNewResults(IEnumerable<MonitoringResult> masterResults,
			IEnumerable<MonitoringResult> newResults);

		/// <summary>
		/// Изменяем главную коллекцию
		/// </summary>
		/// <returns>
		/// В случае изменения коллекции возвращаем true
		/// </returns>
		bool SetResults(List<MonitoringResult> masterResults, IReadOnlyCollection<MonitoringResult> deletedResults,
				IReadOnlyCollection<MonitoringResult> toBeUpdatedResults, IReadOnlyCollection<MonitoringResult> newResults);
	}
}