using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using SitesChecker.DataAccess.Models;
using SitesChecker.Domain;
using SitesChecker.Domain.Infrastructure;

namespace SitesChecker.Tests.IntegrationTests
{	
	[TestFixture]
	public class UrlCheckerIntegrationTest
	{
		public IUrlChecker CreateChecker()
		{
			return new UrlChecker(Substitute.For<ILoggerFactory>(), new ResponseDataProvider());
		}
		
		[Test]
		public void Should_CheckSite_Correctly()
		{
			var checker = CreateChecker();
			var site =new  SiteAvailability()
			{
				Url = "http://google.com"
			};

			var result=checker.Check(site).Result;

			result.IsAvailable.Should().Be(true);
		}
		
		[TestCase("blabla")]
		public void Should_NotCheckSite(string url)
		{
			var checker = CreateChecker();
			var site = new  SiteAvailability()
			{
				Url = url
			};

			var result=checker.Check(site).Result;

			result.IsAvailable.Should().Be(false);
		}
	}
}