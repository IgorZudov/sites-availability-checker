using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SitesChecker.Domain.Models;

namespace SitesChecker.DataAccess
{
	public class DataContext : DbContext, IDataContext
	{
		public DbSet<User> Users { get; set; }

		public DbSet<Site> Sites { get; set; }

		public DbSet<SiteAvailability> SiteAvailabilities { get; set; }

		public Task Create<T>(T entity) where T : class
		{
			return Set<T>().AddAsync(entity);
		}

		public void Update<T>(T entity) where T : class
		{
			base.Update(entity);
		}

		public void Update<T>(T entity, Action<T> updateAction) where T : class
		{
			var set = Set<T>();
			var entityEntry = Entry(entity);
			if (entityEntry.State == EntityState.Detached)
			{
				set.Attach(entity);
			}

			updateAction.Invoke(entity);
		}

		public void Delete<T>(T entity) where T : class
		{
			var set = Set<T>();
			if (Entry(entity).State == EntityState.Detached)
			{
				set.Attach(entity);
			}

			set.Remove(entity);
		}

		public void DeleteRange<T>(ICollection<T> entities) where T : class
		{
			var set = Set<T>();
			foreach (var entity in entities)
			{
				if (Entry(entity).State == EntityState.Detached)
				{
					set.Attach(entity);
				}
			}

			set.RemoveRange(entities);
		}

		public IQueryable<T> Query<T>() where T : class
		{
			return Set<T>();
		}

		public void Commit()
		{
			SaveChanges();
		}

		public Task CommitAsync()
		{
			return SaveChangesAsync();
		}

		public void InitDatabase()
		{
			Database.Migrate();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<SiteAvailability>()
				.HasOne(p => p.Site).WithOne().OnDelete(DeleteBehavior.Cascade);
			
			modelBuilder.Entity<SiteAvailability>().HasKey(u => u.Id);
			modelBuilder.Entity<Site>().HasKey(u => u.Id);
			modelBuilder.Entity<User>().HasKey(k => k.Id);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite(@"Filename=Sites.db");
			optionsBuilder.EnableSensitiveDataLogging();
		}
	}
}