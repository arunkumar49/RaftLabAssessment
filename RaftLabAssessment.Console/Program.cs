using RaftLab.Assessment.Implementation;

namespace RaftLabAssessment.Console
{
	internal class Program
	{
		public async static Task Main(string[] args)
		{
			await Startup.GetUserDetailsAsync();
			await Startup.GetAllUsersAsync();

			await Startup.GetUserDetailsAsync();
			await Startup.GetAllUsersAsync();
		}
	}
}
