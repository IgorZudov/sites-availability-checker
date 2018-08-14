using System.Net;

namespace SitesChecker.Domain.Infrastructure
{
	public interface IResponseDataProvider
	{
		bool IsResponseAvailable(HttpWebResponse response);
	}
}