using InventoryFoxApp.Exceptions;
using InventoryFoxApp.ServiceModels;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace InventoryFoxApp
{
	public class BackofficeHttpRestClient : IBackofficeRestClient
	{
		private readonly HttpClient _httpClient;

		public BackofficeHttpRestClient(IHttpClientFactory httpFact, IOptions<FoxConfig> config)
		{
			_httpClient = httpFact.CreateClient();
			//This line of code crashes the app
			//this.BaseUri = new Uri(config.Value.BackofficeApiUrl);
		}

		public Uri BaseUri
		{
			get; private set;
		}

		public async Task Delete(Uri requestUri)
		{
			if (!requestUri.IsAbsoluteUri)
			{
				requestUri = new Uri(this.BaseUri, requestUri);
			}

			string apiToken = await SecureStorage.GetAsync(Constants.ApiTokenStorageKey);
			HttpRequestMessage deleteMsg = new HttpRequestMessage(HttpMethod.Delete, requestUri);
			if (apiToken != null)
			{
				deleteMsg.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiToken);
			}

			using (HttpResponseMessage res = await _httpClient.SendAsync(deleteMsg))
			{
				if (res.IsSuccessStatusCode)
				{
					return;
				}
				else if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized)
				{
					throw new NotLoggedInException();
				}
				else if (res.StatusCode == System.Net.HttpStatusCode.Forbidden)
				{
					throw new PermissionDeniedException();
				}
				else if (res.StatusCode == System.Net.HttpStatusCode.InternalServerError)
				{
					string problem = await res.Content.ReadAsStringAsync();
					Problem p;
					try
					{
						p = System.Text.Json.JsonSerializer.Deserialize<Problem>(problem, new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
					}
					catch (Exception)
					{
						string c = await res.Content.ReadAsStringAsync();
						throw new ApiErrorException($"Error: {requestUri.ToString()} {c}");
					}

					if (p != null)
					{
						throw new ApiErrorException(p.Detail);
					}
					else
					{
						string c = await res.Content.ReadAsStringAsync();
						throw new ApiErrorException($"Error: {requestUri.ToString()} {c}");
					}
				}
				else
				{
					throw new ApiErrorException($"Calling [DELETE] {requestUri.ToString()} errored with status code {res.StatusCode} ({res.ReasonPhrase})");
				}
			}
		}

		public async Task<TResponse> Get<TResponse>(Uri requestUri)
		{
			if (!requestUri.IsAbsoluteUri)
			{
				requestUri = new Uri(this.BaseUri, requestUri);
			}
			string apiToken = await SecureStorage.GetAsync(Constants.ApiTokenStorageKey);
			HttpRequestMessage getMsg = new HttpRequestMessage(HttpMethod.Get, requestUri);
			if (apiToken != null)
			{
				getMsg.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiToken);
			}

			using (HttpResponseMessage res = await _httpClient.SendAsync(getMsg))
			{
				if (res.IsSuccessStatusCode)
				{
					TResponse val = await res.Content.ReadAsAsync<TResponse>();
					return val;
				}
				else if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized)
				{
					throw new NotLoggedInException();
				}
				else if (res.StatusCode == System.Net.HttpStatusCode.Forbidden)
				{
					throw new PermissionDeniedException();
				}
				else if (res.StatusCode == System.Net.HttpStatusCode.InternalServerError)
				{
					string problem = await res.Content.ReadAsStringAsync();
					Problem p;
					try
					{
						p = System.Text.Json.JsonSerializer.Deserialize<Problem>(problem, new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
					}
					catch (Exception)
					{
						string c = await res.Content.ReadAsStringAsync();
						throw new ApiErrorException($"Error: {requestUri.ToString()} {c}");
					}

					if (p != null)
					{
						throw new ApiErrorException(p.Detail);
					}
					else
					{
						string c = await res.Content.ReadAsStringAsync();
						throw new ApiErrorException($"Error: {requestUri.ToString()} {c}");
					}
				}
				else
				{
					throw new ApiErrorException($"Calling [GET] {requestUri.ToString()} errored with status code {res.StatusCode} ({res.ReasonPhrase})");
				}
			}
		}

		public async Task<TResponse> Post<TResponse, TRequest>(Uri requestUri, TRequest requestBody)
		{
			if (!requestUri.IsAbsoluteUri)
			{
				requestUri = new Uri(this.BaseUri, requestUri);
			}

			string apiToken = await SecureStorage.GetAsync(Constants.ApiTokenStorageKey);
			HttpRequestMessage postMsg = new HttpRequestMessage(HttpMethod.Post, requestUri);
			postMsg.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
			if (apiToken != null)
			{
				postMsg.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiToken);
			}

			using (HttpResponseMessage res = await _httpClient.SendAsync(postMsg))
			{
				if (res.IsSuccessStatusCode)
				{
					TResponse val = await res.Content.ReadAsAsync<TResponse>();
					return val;
				}
				else if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized)
				{
					throw new NotLoggedInException();
				}
				else if (res.StatusCode == System.Net.HttpStatusCode.Forbidden)
				{
					throw new PermissionDeniedException();
				}
				else if (res.StatusCode == System.Net.HttpStatusCode.InternalServerError)
				{
					string problem = await res.Content.ReadAsStringAsync();
					Problem p;
					try
					{
						p = System.Text.Json.JsonSerializer.Deserialize<Problem>(problem, new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
					}
					catch (Exception)
					{
						string c = await res.Content.ReadAsStringAsync();
						throw new ApiErrorException($"Error: {requestUri.ToString()} {c}");
					}

					if (p != null)
					{
						throw new ApiErrorException(p.Detail);
					}
					else
					{
						string c = await res.Content.ReadAsStringAsync();
						throw new ApiErrorException($"Error: {requestUri.ToString()} {c}");
					}
				}
				else
				{
					throw new ApiErrorException($"Calling [POST] {requestUri.ToString()} errored with status code {res.StatusCode} ({res.ReasonPhrase})");
				}
			}
		}

		public async Task PostExpectNoResponse<TRequest>(Uri requestUri, TRequest requestBody)
		{
			if (!requestUri.IsAbsoluteUri)
			{
				requestUri = new Uri(this.BaseUri, requestUri);
			}

			string apiToken = await SecureStorage.GetAsync(Constants.ApiTokenStorageKey);
			HttpRequestMessage postMsg = new HttpRequestMessage(HttpMethod.Post, requestUri);
			postMsg.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
			if (apiToken != null)
			{
				postMsg.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiToken);
			}

			using (HttpResponseMessage res = await _httpClient.SendAsync(postMsg))
			{
				if (res.IsSuccessStatusCode)
				{
					return;
				}
				else if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized)
				{
					throw new NotLoggedInException();
				}
				else if (res.StatusCode == System.Net.HttpStatusCode.Forbidden)
				{
					throw new PermissionDeniedException();
				}
				else if (res.StatusCode == System.Net.HttpStatusCode.InternalServerError)
				{
					string problem = await res.Content.ReadAsStringAsync();
					Problem p = System.Text.Json.JsonSerializer.Deserialize<Problem>(problem, new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
					if (p != null)
					{
						throw new ApiErrorException(p.Detail);
					}
					else
					{
						string c = await res.Content.ReadAsStringAsync();
						throw new ApiErrorException($"Error: {requestUri.ToString()} {c}");
					}
				}
				else
				{
					throw new ApiErrorException($"Calling [POST (NR)] {requestUri.ToString()} errored with status code {res.StatusCode} ({res.ReasonPhrase})");
				}
			}
		}
	}
}
