using System.Threading.Tasks;

namespace SitesChecker.Domain.Infrastructure
{
	public interface IResponseDataProvider
	{
		Task<bool> IsResponseAvailable(string url);
	}
}