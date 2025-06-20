using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Logging;
using RaftLab.Assessment.Interfaces;
using RaftLab.Assessment.Models;
using System.Text.Json;

namespace RaftLab.Assessment.Implementation
{
  public class Startup
  {
	 #region attributes

	 private static IExternalUserService _externalUserService;

	 #endregion

	 #region constructor

		public Startup()
		{
			var host = Host.CreateDefaultBuilder()
			  .ConfigureAppConfiguration((context, config) =>
			  {
				  config.SetBasePath(Directory.GetCurrentDirectory());
				  config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

			  }).ConfigureServices((context, services) =>
			  {
				  NLog.LogManager.Configuration = new NLogLoggingConfiguration(context.Configuration.GetSection("NLog"));
				  services.AddSingleton<IConfiguration>(context.Configuration);
				  services.AddServices(context.Configuration);
			  })
			  .Build();

			_externalUserService = host.Services.GetRequiredService<IExternalUserService>();

		}


		public Startup(IExternalUserService userService)
		{
			_externalUserService = userService;
		}

		#endregion

		#region public methods
		/// <summary>
		/// Get user details by id
		/// </summary>
		/// <returns></returns>
		public async Task<User?> GetUserDetailsAsync(int id)
		{
			return await _externalUserService.GetUserByIdAsync(id);
		}

		/// <summary>
		/// Fetch all the users
		/// </summary>
		/// <returns></returns>
		public async Task<List<List<User>>?> GetAllUsersAsync()
		{
			var usersList = await _externalUserService.GetAllUsersAsync();

			

			return usersList;
		}
		#endregion

  }
}
