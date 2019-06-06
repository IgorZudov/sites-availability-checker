using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SitesChecker.Domain.Infrastructure;

namespace SitesChecker.Core
{
	public class ResponseDataProvider : IResponseDataProvider
	{
		private readonly HttpClient _client;

		public ResponseDataProvider()
		{
			_client = new HttpClient();
		}
	
		public async Task<bool> IsResponseAvailable(string url)
		{
			var response = await _client.GetAsync(url);
			return response.IsSuccessStatusCode;
		}
	}
}