using RaftLab.Assessment.Implementation;
using System.Text.Json;

namespace RaftLabAssessment.Consoles
{
	public class Program
	{
		public async static Task Main(string[] args)
		{
			Startup startup = new Startup();
			Console.Write("Please enter if of an employee : ");
			var id = Convert.ToInt32(Console.ReadLine());
			
			var user = await startup.GetUserDetailsAsync(id);

			var userData = user == null ?
			"We're unable to load the requested data at the moment.\r\nIf this issue persists, please contact the development team for assistance." :
			$"User details : \n First Name : {user.FirstName} \n Last Name : {user.LastName} \n Email : {user.Email} \n Avatar: {user.Avatar}";

			Console.WriteLine(userData);

			var usersList = await startup.GetAllUsersAsync();

			var usersData = usersList?.Count == 0 ?
			"\nWe're unable to load the requested data at the moment.\r\nIf this issue persists, please contact the development team for assistance.\n" :
			$"\nUsers list : {JsonSerializer.Serialize(usersList)}\n";

			Console.WriteLine(usersData);

			// Called twice to check cache functionality
			await startup.GetUserDetailsAsync(id);
			await startup.GetAllUsersAsync();

			Console.WriteLine("Please press enter key to exit");
			Console.ReadLine();
		}
	}
}
