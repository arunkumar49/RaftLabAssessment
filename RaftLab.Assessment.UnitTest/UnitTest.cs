using Moq;
using RaftLab.Assessment.Implementation;
using RaftLab.Assessment.Interfaces;
using RaftLab.Assessment.Models;
using RaftLab.Assessment.UnitTest.Constants;
using System.Text.Json;

namespace RaftLab.Assessment.UnitTest
{
	public class UnitTest
	{
		[Fact]
		public async Task GetUserDetailsAsyncPrintsFirstName()
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

		[Fact]
		public async Task GetUsersList()
		{
			// Arrange
			var mockService = new Mock<IExternalUserService>();

			var deserializedUserslist = JsonSerializer.Deserialize<List<List<User>>>(PaginatedUsers.usersResponse);
			mockService.Setup(s => s.GetAllUsersAsync())  // match the expected argument
					   .ReturnsAsync(deserializedUserslist);

			var startup = new Startup(mockService.Object);

			// Act
			var userList = await startup.GetAllUsersAsync();
			
			var serializedUsersList = JsonSerializer.Serialize(deserializedUserslist);

			// Assert
			Assert.Equal(PaginatedUsers.usersResponse, serializedUsersList);
		}

	}
}