
using System;
using System.Collections.Generic;
using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using SitesChecker.DataAccess.Models;
using SitesChecker.Domain;
using SitesChecker.Domain.Infrastructure;

namespace SitesChecker.Tests.UnitTests
{
	[TestFixture]
	public class SiteCheckerUnitTests
	{
		IUrlChecker GetChecker(IResponseDataProvider responseDataProvider)
		{
			return new UrlChecker(Substitute.For<ILoggerFactory>(), responseDataProvider);
		}

		SiteAvailability GetSite()
		{
			return new SiteAvailability()
			{
				Name = "test",
				Url = "http://vk.com"
			};

		}
		[Test]
		public void Should_TrowNullException_WithNullList()
		{
			var checker = GetChecker(Substitute.For<IResponseDataProvider>());

			Action act = () => checker.Check((List<SiteAvailability>)null);

			act.Should().Throw<ArgumentNullException>();
		}
		[Test]
		public void Should_TrowNullException_WithNullArgument()
		{
			var checker = GetChecker(Substitute.For<IResponseDataProvider>());

			Action act = () =>
			{
				var monitoringResult = checker.Check((SiteAvailability)null).Result;
			};
			
			act.Should().Throw<ArgumentNullException>();
		}

		[Test]
		public void Should_ReturnUnsusccesResult()
		{
			var provider = Substitute.For<IResponseDataProvider>();
			provider.IsResponseAvailable(Arg.Any<HttpWebResponse>())
				.Returns(false);
			var checker = GetChecker(provider);

			var result=checker.Check(GetSite());

			result.Result.IsAvailable.Should().Be(false);
		}
		
		[Test]
		public void Should_ReturnSuccessResult()
		{
			var provider = Substitute.For<IResponseDataProvider>();
			provider.IsResponseAvailable(Arg.Any<HttpWebResponse>())
				.Returns(true);
			var checker = GetChecker(provider);

			var result = checker.Check(GetSite());

			result.Result.IsAvailable.Should().Be(true);
		}

		
	}
}