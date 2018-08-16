using System.Net;
using SitesChecker.Domain.Infrastructure;

namespace SitesChecker.Domain
{
	public class ResponseDataProvider: IResponseDataProvider
	{
		public bool IsResponseAvailable(string url)
		{
			var request = WebRequest.Create(url);
			var response = (HttpWebResponse)(request.GetResponseAsync().Result);
			return !(response == null || response.StatusCode != HttpStatusCode.OK);
		}
	}
}