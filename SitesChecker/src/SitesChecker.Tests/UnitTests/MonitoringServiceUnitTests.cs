using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using SitesChecker.Core;
using SitesChecker.DataAccess;
using SitesChecker.DataAccess.Models;
using SitesChecker.Domain;
using SitesChecker.Domain.Infrastructure;

namespace SitesChecker.Tests.UnitTests
{
    [TestFixture]
    public class MonitoringServiceUnitTests
    {
        public IMonitoringService GetService(IDataContext dataContext,IUrlChecker urlChecker)
        {
            return new MonitoringHostedService(Substitute.For<ILoggerFactory>(),dataContext, urlChecker);
        }

        private Site GetSite()
        {
            return new Site()
            {
                Name = "test",
                Url = "ya.com"
            };
        }

	    [Test]
	    public void Should_ReturnMinDelay()
	    {
		    var dbContext = Substitute.For<IDataContext>();
		    dbContext.Query<Site>().Returns((new List<Site>() {new Site() {UpdateDelay = 5}, new Site() {UpdateDelay = 15}}).AsQueryable());
		    var service = GetService(dbContext, Substitute.For<IUrlChecker>());

		    var delay=service.GetTimeDelay();

		    delay.Should().Be(5);
	    }
    }
}