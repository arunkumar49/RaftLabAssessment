using RaftLab.Assessment.Implementation;
using System.Text.Json;

namespace RaftLabAssessment.Consoles
{
	public class Program
	{
		public async static Task Main(string[] args)
		{
			Startup startup = new Startup();

			// Fetch user details based on user id
			Console.Write("Please enter if of an employee : ");
			var input = Console.ReadLine();
			var id = int.TryParse(input, out var value) ? value : 0;

			var user = await startup.GetUserDetailsAsync(id);

			var userData = user == null ?
			"We're unable to load the requested data at the moment.\r\nIf this issue persists, please contact the development team for assistance." :
			$"User details : \n First Name : {user.FirstName} \n Last Name : {user.LastName} \n Email : {user.Email} \n Avatar: {user.Avatar}";

			Console.WriteLine(userData);

			// Fetch list of all the users.
			var usersList = await startup.GetAllUsersAsync();

			var usersData = usersList?.Count == 0 ?
			"\nWe're unable to load the requested data at the moment.\r\nIf this issue persists, please contact the development team for assistance.\n" :
			$"\nUsers list : {JsonSerializer.Serialize(usersList)}\n";

			Console.WriteLine(usersData);

			// Called twice to check cache functionality
			await startup.GetUserDetailsAsync(id);
			await startup.GetAllUsersAsync();

			// To prevent console closing quickly in release mode.
			Console.WriteLine("Please press enter key to exit");
			Console.ReadLine();
		}
	}
}
