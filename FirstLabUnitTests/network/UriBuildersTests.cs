using System;
using System.Collections.Specialized;
using System.Globalization;
using FirstLab.network;
using NUnit.Framework;
using Xamarin.Essentials;

namespace FirstLabUnitTests.network
{
    public class UriBuildersTests
    {
        [Test]
        public void NameValueCollectionShouldContainsSpecifiedId()
        {
            var id = 1111;
            var result = Network.ByInstallationId(id);
            Assert.AreEqual(id.ToString(), result.Get("installationId"));
        }

        [Test]
        public void NameValueCollectionShouldContainSpecifiedLocationValues()
        {
            var location = new Location(52.2297, 21.0122);
            var result = Network.NearestInstallationsQuery(location, 1);
            Assert.AreEqual(location.Latitude.ToString(CultureInfo.InvariantCulture), result.Get("lat"));
            Assert.AreEqual(location.Longitude.ToString(CultureInfo.InvariantCulture), result.Get("lng"));
            Assert.AreEqual("-1", result.Get("maxDistanceKM"));
            Assert.AreEqual("1", result.Get("maxResults"));
        }

        [Test]
        public void ShouldReturnUriBuilderContainingCorrectBaseUrl()
        {
            var baseAddress = new Uri("https://example.com");
            var endpoint = "some/endpoint";
            var result = Network.CreateUriBuilder(baseAddress)(endpoint)(new NameValueCollection());
            Assert.IsTrue(result.Uri.ToString().StartsWith(baseAddress.ToString()));
        }

        [Test]
        public void ShouldReturnUriBuilderContainingCorrectPath()
        {
            var baseAddress = new Uri("https://example.com");
            var endpoint = "some/endpoint";
            var result = Network.CreateUriBuilder(baseAddress)(endpoint)(new NameValueCollection());
            Assert.AreEqual("/" + endpoint, result.Path);
        }
    }
}