using FluentAssertions;
using NUnit.Framework;
using SitesChecker.Domain.Utils;

namespace SitesChecker.Tests.UnitTests
{
    [TestFixture]
    public class UrlHelperTests
    {
        [TestCase("http://facebook.com")]
        [TestCase("https://silence.com.ua")]
        public void Should_PositivelyValidateUrls(string url)
        {
            var result = UrlHelper.IsUrlValid(url);

            result.Should().Be(true);
        }

        [TestCase("facebook.com")]
        public void Should_NegativelyValidateUrls(string url)
        {
            var result = UrlHelper.IsUrlValid(url);

            result.Should().Be(false);
        }
    }
}