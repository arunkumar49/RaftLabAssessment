using Moq;
using RaftLab.Assessment.Implementation;
using RaftLab.Assessment.Interfaces;
using RaftLab.Assessment.Models;
using Xunit;

namespace RaftLab.Assessment.UnitTest
{
	public class Program
	{
		static async Task Main(string[] args)
		{
			await GetUserDetailsAsyncPrintsFirstName();
		}


		/// <summary>
		/// To test 
		/// </summary>
		/// <returns></returns>
		[Fact]
		public static async Task GetUserDetailsAsyncPrintsFirstName()
		{
			// Arrange
			var mockService = new Mock<IExternalUserService>();
			mockService.Setup(s => s.GetUserByIdAsync(2))  // match the expected argument
					   .ReturnsAsync(new User { FirstName = "Alice", LastName = "Smith" });

			var startup = new Startup(mockService.Object);

			// Act
			var user = await startup.GetUserDetailsAsync(2);

			// Assert
			Assert.Equal("Alice", user?.FirstName);
		}




	}
}
