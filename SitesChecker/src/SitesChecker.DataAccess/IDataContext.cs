using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SitesChecker.DataAccess
{
	public interface IDataContext : IDisposable
	{
		void Create<T>(T entity) where T : class;

		void Update<T>(T entity) where T : class;

		void Update<T>(T entity, Action<T> updateAction) where T : class;

		void Delete<T>(T entity) where T : class;

		void DeleteRange<T>(ICollection<T> entity) where T : class;

		IQueryable<T> Query<T>() where T : class;

		void Commit();

		Task CommitAsync();

		void InitDatabase();
	}
}