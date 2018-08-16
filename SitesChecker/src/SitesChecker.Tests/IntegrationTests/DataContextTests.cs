using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SitesChecker.DataAccess;
using SitesChecker.Domain.Models;

namespace SitesChecker.Tests.IntegrationTests
{
	[TestFixture]
	public class DataContextTests
	{
		IDataContext GetContext()
		{
			var context = new DataContext();
			context.InitDatabase();
			return context;
		}

		static User GetUser()
		{
			return new User
			{
				Login = "testlogin",
				Password = "123",
				Role = "admin"
			};
		}

		static Site GetSiteAvailability()
		{
			return new Site
			{
				Name = "testSite",
				Url = "http://ya.com"
			};
		}

		[Test]
		public void Should_CreateSite()
		{
			var context = GetContext();
			var site = GetSiteAvailability();
			context.Create(site);
			context.CommitAsync();
			var fromDbUser = context.Query<Site>().FirstOrDefault(_ => _.Id == site.Id);
			fromDbUser.Should().NotBeNull();
			context.Delete(site);
		}

		[Test]
		public void Should_CreateUser()
		{
			var context = GetContext();
			var user = GetUser();
			context.Create(user);
			context.CommitAsync();
			var fromDbUser = context.Query<User>().FirstOrDefault(_ => _.Login == user.Login);
			fromDbUser.Should().NotBeNull();
			context.Delete(user);
		}

		[Test]
		public void Should_DeleteSite()
		{
			var context = GetContext();
			var site = GetSiteAvailability();
			context.Create(site);
			context.CommitAsync();
			context.Delete(site);
			context.CommitAsync();
			var fromDbUser = context.Query<Site>().FirstOrDefault(_ => _.Id == site.Id);
			fromDbUser.Should().BeNull();
		}

		[Test]
		public void Should_DeleteUser()
		{
			var context = GetContext();
			var user = GetUser();
			context.Create(user);
			context.CommitAsync();
			context.Delete(user);
			context.CommitAsync();
			var fromDbUser = context.Query<User>().FirstOrDefault(_ => _.Id == user.Id);
			fromDbUser.Should().BeNull();
		}

		[Test]
		public void Should_UpdateSite()
		{
			var context = GetContext();
			var site = GetSiteAvailability();
			context.Create(site);
			context.CommitAsync();
			const string newUrl = "hhtp://google.com";
			site.Url = newUrl;
			context.Update(site);
			context.CommitAsync();
			var fromDbUser = context.Query<Site>().First(_ => _.Id == site.Id);
			fromDbUser.Url.Should().Be(newUrl);
			context.Delete(fromDbUser);
		}

		[Test]
		public void Should_UpdateUser()
		{
			var context = GetContext();
			var user = GetUser();
			context.Create(user);
			context.CommitAsync();
			const string newPass = "newPa$word";
			user.Password = newPass;
			context.Update(user);
			context.CommitAsync();
			var fromDbUser = context.Query<User>().First(_ => _.Id == user.Id);
			fromDbUser.Password.Should().Be(newPass);
			context.Delete(fromDbUser);
		}
	}
}