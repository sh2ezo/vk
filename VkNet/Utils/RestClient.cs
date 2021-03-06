using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VkNet.Abstractions.Utils;

namespace VkNet.Utils
{
	/// <inheritdoc />
	[UsedImplicitly]
	public class RestClient : IRestClient
	{
		private readonly HttpClient _httpClient;

		private readonly ILogger<RestClient> _logger;

		/// <inheritdoc />
		public RestClient(HttpClient httpClient, ILogger<RestClient> logger)
		{
			_httpClient = httpClient;
			_logger = logger;
		}

		/// <inheritdoc />
		[Obsolete("Use HttpClientFactory to configure proxy.")]
		public IWebProxy Proxy { get; set; }

		/// <inheritdoc />
		[Obsolete("Use HttpClientFactory to configure timeout.")]
		public TimeSpan Timeout { get; set; }

		/// <inheritdoc />
		public Task<HttpResponse<string>> GetAsync(Uri uri, IEnumerable<KeyValuePair<string, string>> parameters)
		{
			var queries = parameters
				.Where(parameter => !string.IsNullOrWhiteSpace(parameter.Value))
				.Select(parameter => $"{parameter.Key.ToLowerInvariant()}={parameter.Value}");

			var url = new UriBuilder(uri)
			{
				Query = string.Join("&", queries)
			};

			_logger?.LogDebug($"GET request: {url.Uri}");

			var request = new HttpRequestMessage(HttpMethod.Get, uri);

			return CallAsync(httpClient => httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead));
		}

		/// <inheritdoc />
		public Task<HttpResponse<string>> PostAsync(Uri uri, IEnumerable<KeyValuePair<string, string>> parameters)
		{
			if (_logger != null)
			{
				var json = JsonConvert.SerializeObject(parameters);
				_logger.LogDebug($"POST request: {uri}{Environment.NewLine}{Utilities.PrettyPrintJson(json)}");
			}

			var content = new FormUrlEncodedContent(parameters);

			var request = new HttpRequestMessage(HttpMethod.Post, uri) { Content = content };

			return CallAsync(httpClient => httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead));
		}

		private async Task<HttpResponse<string>> CallAsync(Func<HttpClient, Task<HttpResponseMessage>> method)
		{
			var response = await method(_httpClient).ConfigureAwait(false);

			var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

			_logger?.LogDebug($"Response:{Environment.NewLine}{Utilities.PrettyPrintJson(content)}");
			var url = response.RequestMessage.RequestUri.ToString();

			return response.IsSuccessStatusCode
				? HttpResponse<string>.Success(response.StatusCode, content, url)
				: HttpResponse<string>.Fail(response.StatusCode, content, url);
		}

		/// <inheritdoc />
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~RestClient()
		{
			Dispose(false);
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing">
		/// <c> true </c> to release both managed and unmanaged resources; <c> false </c>
		/// to release only
		/// unmanaged resources.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_httpClient?.Dispose();
			}
		}
	}
}