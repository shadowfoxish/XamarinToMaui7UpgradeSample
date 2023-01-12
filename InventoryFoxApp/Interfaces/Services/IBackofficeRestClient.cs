using System;
using System.Threading.Tasks;

namespace InventoryFoxApp
{
	public interface IBackofficeRestClient
	{
		Uri BaseUri
		{
			get;
		}

		Task Delete(Uri request);

		Task<TResponse> Get<TResponse>(Uri requestUri);

		Task<TResponse> Post<TResponse, TRequest>(Uri requestUri, TRequest requestBody);

		Task PostExpectNoResponse<TRequest>(Uri requestUri, TRequest requestBody);
	}
}
