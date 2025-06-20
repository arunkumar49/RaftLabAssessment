using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using NLog;
using RaftLab.Assessment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RaftLab.Assessment.Infrastructure
{
  public class HttpClientFactoryHelper<T> : IHttpClientFactoryHelper<T>
  {
    #region attributes

        private readonly HttpClient _httpReqresClient;
        private readonly IMemoryCache _memoryCache;
		private readonly IConfiguration _config;
		private readonly static Logger _log = LogManager.GetCurrentClassLogger();
        private readonly string _configClearCache = "ClearCache";
        private readonly string _configCacheTimeKey = "CacheExpirationTime";

		#endregion

    #region constructors

		public HttpClientFactoryHelper(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache, IConfiguration config)
        {
            _httpReqresClient = httpClientFactory.CreateClient("ReqresClient");
            _memoryCache = memoryCache;
            _config = config;
        }

    #endregion

    #region public methods

    /// <summary>
    /// Common method send all the HTTP get requests
    /// </summary>
    /// <param name="childUrl"></param>
    /// <returns></returns>
    public async Task<T?> SendGetAsync(string childUrl)
{
    try
    {
        // Given option to clear cache for user
        if (_config.GetValue<bool>(_configClearCache))
        {
            _memoryCache.Remove(childUrl);
        }

        if (_memoryCache.TryGetValue(childUrl, out T? cachedValue))
        {
            _log.Trace($"Cache hit for {childUrl}");
            return cachedValue;
        }
        
        HttpResponseMessage response = await _httpReqresClient.GetAsync(childUrl);

        if (!response.IsSuccessStatusCode)
        {
            _log.Warn($"GET request to {childUrl} failed with status code {response.StatusCode}.");
            return default;
        }

        using var contentStream = await response.Content.ReadAsStreamAsync();
        var data = await JsonSerializer.DeserializeAsync<T>(contentStream);

        if (data != null)
        {
            int cacheTime = _config.GetValue<int>(_configCacheTimeKey);
			// Set cache with expiration
			_memoryCache.Set(childUrl, data, TimeSpan.FromMinutes(cacheTime));
        }

        return data;
    }

    catch (HttpRequestException httpEx)
    {
        _log.Error(httpEx, $"HTTP error occurred while sending GET request to {childUrl}");
    }
    catch (TaskCanceledException canceledEx)
    {
        _log.Error(canceledEx, $"Request to {childUrl} timed out.");
    }
    catch (JsonException jsonEx)
    {
        _log.Error(jsonEx, $"Deserialization error for response from {childUrl}");
    }
    catch (Exception ex)
    {
        _log.Error(ex, $"Unexpected error occurred while sending GET request to {childUrl}");
    }

    return default;
}

    #endregion

  }
}
