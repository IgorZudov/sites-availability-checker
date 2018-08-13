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
				Url = "http://google.com"
			};

			var result=checker.Check(site).Result;

			result.IsAvailable.Should().Be(true);
		}
		
		[TestCase("http://google.testtest")]
		[TestCase("facebook.com")]
		[TestCase("htps://silence.com.ua")]
		[TestCase("ftp://123.ru")]
		[TestCase("http://123.456")]
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