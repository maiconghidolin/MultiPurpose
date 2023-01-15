using Microsoft.AspNetCore.Mvc;
using MultiPurposeProject.Controllers;
using MultiPurposeProject.Entities;
using MultiPurposeProject.Models.Users;
using MultiPurposeProjectUnitTest.Services;
using MultiPurposeProjectUnitTest.Attributes;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace MultiPurposeProjectUnitTest
{

    [TestCaseOrderer("MultiPurposeProjectUnitTest.PriorityOrderer", "MultiPurposeProjectUnitTest")]
    public class UsersControllerTest
    {

        private static UsersController _usersController;

        public UsersControllerTest()
        {
            var userService = new UserServiceFake();
            _usersController = new UsersController(userService);
        }

        [Fact, TestPriority(1)]
        public void WhenCallingRegister_ThenReturnsOK()
        {
            // Arrange.
            var registerRequest = new RegisterRequest()
            {
                Name = "Other User",
                Username = "otherUser",
                Password = "otherUser123"
            };

            // Act.
            var result = _usersController.Register(registerRequest);

            // Assert.
            var okResult = Assert.IsType<OkObjectResult>(result as OkObjectResult);
            var user = okResult.Value as User;

            Assert.Equal("Other User", user.Name);
            Assert.Equal("otherUser", user.Username);
        }

        [Fact, TestPriority(2)]
        public void WhenCallingRegister_ThenReturnsBadRequest()
        {
            // Arrange.
            object? expectedContent = new { message = "Username 'testUser' is already taken" };

            var registerRequest = new RegisterRequest()
            {
                Name = "Test User",
                Username = "testUser",
                Password = "testUser123"
            };

            // Act.
            var result = _usersController.Register(registerRequest);

            // Assert.
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result as BadRequestObjectResult);
            object? responseContent = badRequestResult.Value as object;


            Assert.Equivalent(expectedContent, responseContent);

        }

        [Fact, TestPriority(3)]
        public void WhenCallingAuthenticate_ThenReturnsBadRequest()
        {
            // Arrange.
            object? expectedContent = new { message = "Username or password is incorrect" };

            var content = new AuthenticateRequest()
            {
                Username = "testures",
                Password = "testures123"
            };

            // Act.
            var result = _usersController.Authenticate(content);

            // Assert.
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result as BadRequestObjectResult);
            object? responseContent = badRequestResult.Value as object;

            Assert.Equivalent(expectedContent, responseContent);

        }

        [Fact, TestPriority(4)]
        public void WhenCallingAuthenticate_ThenReturnsOK()
        {
            // Arrange.
            var content = new AuthenticateRequest()
            {
                Username = "testUser",
                Password = "testUser123"
            };

            // Act.
            var result = _usersController.Authenticate(content);

            // Assert.
            var okResult = Assert.IsType<OkObjectResult>(result as OkObjectResult);
            var response = okResult.Value as AuthenticateResponse;

            Assert.Equal("Test User", response.Name);
            Assert.Equal("testUser", response.Username);
            Assert.False(string.IsNullOrEmpty(response.Token));

        }

        [Fact, TestPriority(5)]
        public void WhenCallingGetAll_ThenReturnsOK()
        {
            // Arrange.

            // Act.
            var result = _usersController.GetAll();

            // Assert.
            var okResult = Assert.IsType<OkObjectResult>(result as OkObjectResult);

            var users = okResult.Value as List<User>;

            Assert.Equal(2, users.Count);

        }

        [Fact, TestPriority(6)]
        public void WhenCallingGetById_ThenReturnsOK()
        {
            // Arrange.
            var expectedContent = new User()
            {
                Name = "Test User",
                Username = "testUser"
            };

            // Act.
            var result = _usersController.GetById(1);

            // Assert.
            var okResult = Assert.IsType<OkObjectResult>(result as OkObjectResult);
            var response = okResult.Value as User;

            Assert.Equal(expectedContent.Name, response.Name);
            Assert.Equal(expectedContent.Username, response.Username);
        }

        [Fact, TestPriority(7)]
        public void WhenCallingGetById_ThenReturnsNotFound()
        {
            // Arrange.
            object? expectedContent = new { message = "User not found" };

            // Act.
            var result = _usersController.GetById(0);

            // Assert.
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result as NotFoundObjectResult);
            object? responseContent = notFoundResult.Value as object;

            Assert.Equivalent(expectedContent, responseContent);
        }

        [Fact, TestPriority(8)]
        public void WhenCallingUpdate_ThenReturnsNotFound()
        {
            // Arrange.
            object? expectedContent = new { message = "User not found" };

            var content = new UpdateRequest()
            {
                Name = "Test User Updated",
                Username = "testUser",
                Password = "testUser123"
            };

            // Act.
            var result = _usersController.Update(0, content);

            // Assert.
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result as NotFoundObjectResult);
            object? responseContent = notFoundResult.Value as object;

            Assert.Equivalent(expectedContent, responseContent);
        }

        [Fact, TestPriority(9)]
        public void WhenCallingUpdate_ThenReturnsOk()
        {
            // Arrange.
            object? expectedContent = new { message = "User updated successfully" };

            var content = new UpdateRequest()
            {
                Name = "Test User Updated",
                Username = "testUser",
                Password = "testUser123"
            };

            // Act.
            var result = _usersController.Update(1, content);

            // Assert.
            var okResult = Assert.IsType<OkObjectResult>(result as OkObjectResult);
            object? responseContent = okResult.Value as object;

            Assert.Equivalent(expectedContent, responseContent);
        }

        [Fact, TestPriority(10)]
        public void WhenCallingDelete_ThenReturnsNotFound()
        {
            // Arrange.
            object? expectedContent = new { message = "User not found" };

            // Act.
            var result = _usersController.Delete(0);

            // Assert.
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result as NotFoundObjectResult);
            object? responseContent = notFoundResult.Value as object;

            Assert.Equivalent(expectedContent, responseContent);
        }

        [Fact, TestPriority(11)]
        public void WhenCallingDelete_ThenReturnsOK()
        {
            // Arrange.
            object? expectedContent = new { message = "User deleted successfully" };

            // Act.
            var result = _usersController.Delete(1);

            // Assert.
            var okResult = Assert.IsType<OkObjectResult>(result as OkObjectResult);
            object? responseContent = okResult.Value as object;

            Assert.Equivalent(expectedContent, responseContent);
        }

    }

}