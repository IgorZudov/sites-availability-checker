using System.Net;
using SitesChecker.Domain.Infrastructure;

namespace SitesChecker.Domain
{
	public class ResponseDataProvider: IResponseDataProvider
	{
		public bool IsResponseAvailable(HttpWebResponse response)
		{
			return !(response == null || response.StatusCode != HttpStatusCode.OK);
		}
	}
}