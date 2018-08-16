namespace SitesChecker.Domain.Infrastructure
{
	public interface IResponseDataProvider
	{
		bool IsResponseAvailable(string url);
	}
}