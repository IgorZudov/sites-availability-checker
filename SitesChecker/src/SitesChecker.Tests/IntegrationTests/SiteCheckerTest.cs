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
	public class SiteCheckerTest
	{
		public ISiteChecker CreateChecker()
		{
			return new SiteChecker(Substitute.For<ILoggerFactory>());
		}
		[Test]
		public void Should_CheckSite_Correctly()
		{
			var checker = CreateChecker();
			var site =new  SiteAvailability()
			{
				Url = "google.com"
			};

			var result=checker.Check(site).Result;

			result.IsAvailable.Should().Be(true);
		}
	}
}