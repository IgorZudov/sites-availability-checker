using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.Core;
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

        private SiteAvailability GetSite()
        {
            return new SiteAvailability()
            {
                Name = "test",
                Url = "ya.com"
            };
        }
        

     
        
        
        [Test]
        public void Should_InvokeChangeEvent()
        {
            var checker = Substitute.For<IUrlChecker>();
            checker.Check(Arg.Any<List<SiteAvailability>>()).Returns( new List<MonitoringResult>(){new MonitoringResult(new SiteAvailability(), true)});
            
            var dataContext = Substitute.For<IDataContext>();
            dataContext.Query<SiteAvailability>().Returns(new  List<SiteAvailability>(){GetSite()}.AsQueryable());
			
            var service = GetService(dataContext, checker);
          
            using (var monitoredSubject = service.Monitor())
            {
	            service.Update().Wait();
				monitoredSubject.Should().Raise("MonitoringResultsChanged");
            }
        }

        [Test]
        public void Should_ReturnEmptyResults()
        {
            var service = GetService(Substitute.For<IDataContext>(), Substitute.For<IUrlChecker>());

            var results = service.GetResults();

            results.Should().BeEmpty();
        }
        
        [Test]
        public void Should_ReturnResults()
        {
            var checker = Substitute.For<IUrlChecker>();
            checker.Check(Arg.Any<List<SiteAvailability>>()).Returns(new List<MonitoringResult>(){new MonitoringResult(new SiteAvailability(), true)});
            
            var dataContext = Substitute.For<IDataContext>();
            dataContext.Query<SiteAvailability>().Returns(new  List<SiteAvailability>(){GetSite()}.AsQueryable());
			
            var service = GetService(dataContext, checker);
            service.Update().Wait();

            var result = service.GetResults();

            result.Should().NotBeNullOrEmpty();

        }
    }
}