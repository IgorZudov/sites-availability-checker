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
        public IMonitoringService GetService(IDataContext dataContext,IUrlChecker urlChecker, IMonitoringResultsComparer comparer)
        {
	        var delay = Substitute.For<IOptionsSnapshot<RequestsDelay>>();
	        delay.Value.Returns(new RequestsDelay()
	        {
		        Delay = 5
	        });
            return new MonitoringHostedService(Substitute.For<ILoggerFactory>(),dataContext, urlChecker,comparer, delay);
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
        public void Should_InvokeChangeEvent_FirstTime()
        {
            var checker = Substitute.For<IUrlChecker>();
            checker.Check(Arg.Any<List<SiteAvailability>>()).Returns( new List<MonitoringResult>(){new MonitoringResult(new SiteAvailability(), true)});
            var dataContext = Substitute.For<IDataContext>();
            dataContext.Query<SiteAvailability>().Returns(new  List<SiteAvailability>(){GetSite()}.AsQueryable());
            var service = GetService(dataContext, checker, Substitute.For<IMonitoringResultsComparer>());
    
            using (var monitoredSubject = service.Monitor())
            {
                service.StartAsync(new CancellationToken());
                
                monitoredSubject.Should().Raise("MonitoringResultsChanged");
            }
        }
        
        
        [Test]
        public void Should_InvokeChangeEvent()
        {
            var checker = Substitute.For<IUrlChecker>();
            checker.Check(Arg.Any<List<SiteAvailability>>()).Returns( new List<MonitoringResult>(){new MonitoringResult(new SiteAvailability(), true)});
            
            var dataContext = Substitute.For<IDataContext>();
            dataContext.Query<SiteAvailability>().Returns(new  List<SiteAvailability>(){GetSite()}.AsQueryable());

            var comparer = Substitute.For<IMonitoringResultsComparer>();
            comparer.SetResults(Arg.Any<List<MonitoringResult>>(),
                Arg.Any<List<MonitoringResult>>(),
                Arg.Any<List<MonitoringResult>>(),
                Arg.Any<List<MonitoringResult>>()).ReturnsForAnyArgs(true);
            
            var service = GetService(dataContext, checker, comparer);
            service.StartAsync(new CancellationToken());
            
            using (var monitoredSubject = service.Monitor())
            {
                //todo need time provider
                Task.Delay(TimeSpan.FromSeconds(6)).Wait();
                monitoredSubject.Should().Raise("MonitoringResultsChanged");
            }
        }

        [Test]
        public void Should_ReturnEmptyResults()
        {
            var service = GetService(Substitute.For<IDataContext>(), Substitute.For<IUrlChecker>(),
                Substitute.For<IMonitoringResultsComparer>());

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

            var comparer = Substitute.For<IMonitoringResultsComparer>();
           
            var service = GetService(dataContext, checker, comparer);
            service.StartAsync(new CancellationToken());

            var result = service.GetResults();

            result.Should().NotBeNullOrEmpty();

        }
    }
}