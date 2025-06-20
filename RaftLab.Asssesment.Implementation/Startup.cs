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

		static Startup()
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

		#endregion

	 #region public methods
		/// <summary>
		/// Get user details by id
		/// </summary>
		/// <returns></returns>
		public static async Task GetUserDetailsAsync()
		{
			var user = await _externalUserService.GetUserByIdAsync();

			var userData = user == null ?
			"We're unable to load the requested data at the moment.\r\nIf this issue persists, please contact the development team for assistance." :
			$"User details : \n First Name : {user.FirstName} \n Last Name : {user.LastName} \n Email : {user.Email} \n Avatar: {user.Avatar}";

			Console.WriteLine(userData);
		}

		/// <summary>
		/// Fetch all the users
		/// </summary>
		/// <returns></returns>
		public static async Task GetAllUsersAsync()
		{
			var usersList = await _externalUserService.GetAllUsersAsync();

			var usersData = usersList.Count == 0 ?
			"\nWe're unable to load the requested data at the moment.\r\nIf this issue persists, please contact the development team for assistance.\n" :
			$"\nUsers list : {JsonSerializer.Serialize(usersList)}\n";

			Console.WriteLine(usersData);
		}
		#endregion

  }
}
