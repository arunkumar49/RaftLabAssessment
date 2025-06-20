using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RaftLab.Assessment.Infrastructure;
using RaftLab.Assessment.Interfaces;
using RaftLab.Assessment.Services;
using Microsoft.Extensions.Caching.Memory;
using Polly.Extensions.Http;
using Polly;
using RaftLabsAssignment.Services;

namespace RaftLab.Assessment.Implementation
{
  public static class DependencyInjection
  {

   #region attributes

    private static readonly string _configBaseUrl = "ReqresApi:BaseUrl";

    #endregion

   #region public method

    /// <summary>
    /// Add required services to implement DI
    /// </summary>
    /// <param name="services"></param>
    /// <returns>services</returns>
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddTransient<IGlobalErrorHandlingService, GlobalErrorHandlingService>();

        services.AddHttpClient("ReqresClient", client =>
        {
            string baseUrl = config.GetValue<string>(_configBaseUrl);
            client.BaseAddress = new Uri(baseUrl);
        })
        .SetHandlerLifetime(TimeSpan.FromMinutes(5))
        .AddPolicyHandler(GetRetryPolicy());

		services.AddMemoryCache();
        services.AddTransient(typeof(IHttpClientFactoryHelper<>), typeof(HttpClientFactoryHelper<>));
        services.AddTransient<IExternalUserService, ExternalUserService>();
        return services;

    }

		static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
		{
			return HttpPolicyExtensions
				.HandleTransientHttpError()
				.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
				.WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,retryAttempt)));
		}

		#endregion

	}
}
