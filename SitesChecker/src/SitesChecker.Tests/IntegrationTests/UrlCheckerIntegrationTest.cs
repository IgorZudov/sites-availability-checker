using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using SitesChecker.Core;
using SitesChecker.Domain.Infrastructure;
using SitesChecker.Domain.Models;

namespace SitesChecker.Tests.IntegrationTests
{
	[TestFixture]
	public class UrlCheckerIntegrationTest
	{
		public IUrlChecker CreateChecker()
		{
			return new UrlChecker(Substitute.For<ILoggerFactory>(), new ResponseDataProvider());
		}

		[TestCase("blabla")]
		public void Should_NotCheckSite(string url)
		{
			var checker = CreateChecker();
			var site = new Site
			{
				Url = url
			};
			var result = checker.Check(new List<Site> {site});
			result.First().IsAvailable.Should().Be(false);
		}

		[Test]
		public void Should_CheckSite_Correctly()
		{
			var checker = CreateChecker();
			var site = new Site
			{
				Url = "http://google.com"
			};
			var result = checker.Check(new List<Site> {site});
			result.First().IsAvailable.Should().Be(true);
		}
	}
}