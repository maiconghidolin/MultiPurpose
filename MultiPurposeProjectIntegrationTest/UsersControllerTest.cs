using MultiPurposeProject.Entities;
using MultiPurposeProject.Models.Users;
using MultiPurposeProjectIntegrationTest.Attributes;
using MultiPurposeProjectIntegrationTest.Helpers;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace MultiPurposeProjectIntegrationTest
{

    [TestCaseOrderer("MultiPurposeProjectIntegrationTest.PriorityOrderer", "MultiPurposeProjectIntegrationTest")]
    public class UsersControllerTest : IClassFixture<TestingWebAppFactory<Program>>
    {

        private readonly HttpClient _httpClient;
        public static readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

        public UsersControllerTest(TestingWebAppFactory<Program> factory)
        {
            _httpClient = factory.CreateClient();
        }

        [Fact, TestPriority(1)]
        public async Task WhenCallingRegister_ThenReturnsOK()
        {
            // Arrange.
            var expectedStatusCode = System.Net.HttpStatusCode.OK;
            var stopwatch = Stopwatch.StartNew();

            var expectedContent = new User()
            {
                Name = "Test User",
                Username = "testuser"
            };

            // Act.
            var response = await RegisterUser();

            // Assert.
            var responseStream = await response.Content.ReadAsStreamAsync();
            var responseContent = await JsonSerializer.DeserializeAsync<User>(responseStream, _jsonSerializerOptions);

            TestHelper.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);

            Assert.Equal(expectedContent.Name, responseContent.Name);
            Assert.Equal(expectedContent.Username, responseContent.Username);
        }

        [Fact, TestPriority(2)]
        public async Task WhenCallingRegister_ThenReturnsBadRequest()
        {
            // Arrange.
            var expectedStatusCode = System.Net.HttpStatusCode.BadRequest;
            var expectedContent = new { message = "Username 'testuser' is already taken" };
            var stopwatch = Stopwatch.StartNew();

            // Act.
            var response = await RegisterUser();

            // Assert.
            await TestHelper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact, TestPriority(3)]
        public async Task WhenCallingAuthenticate_ThenReturnsBadRequest()
        {
            // Arrange
            var expectedStatusCode = System.Net.HttpStatusCode.BadRequest;
            var stopwatch = Stopwatch.StartNew();

            var expectedContent = new { message = "Username or password is incorrect" };

            var content = new AuthenticateRequest()
            {
                Username = "testures",
                Password = "testures123"
            };

            // Act
            var response = await _httpClient.PostAsync("/Users/authenticate", TestHelper.GetJsonStringContent(content));

            // Assert
            await TestHelper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact, TestPriority(4)]
        public async Task WhenCallingAuthenticate_ThenReturnsOK()
        {
            // Arrange
            var expectedStatusCode = System.Net.HttpStatusCode.OK;
            var stopwatch = Stopwatch.StartNew();

            var expectedContent = new AuthenticateResponse()
            {
                Name = "Test User",
                Username = "testuser"
            };

            var content = new AuthenticateRequest()
            {
                Username = "testuser",
                Password = "testuser123"
            };

            // Act
            var response = await _httpClient.PostAsync("/Users/authenticate", TestHelper.GetJsonStringContent(content));

            // Assert
            var responseStream = await response.Content.ReadAsStreamAsync();
            var responseContent = await JsonSerializer.DeserializeAsync<AuthenticateResponse>(responseStream, _jsonSerializerOptions);

            TestHelper.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);

            Assert.Equal(expectedContent.Name, responseContent.Name);
            Assert.Equal(expectedContent.Username, responseContent.Username);

        }

        [Fact, TestPriority(5)]
        public async Task WhenCallingGetAll_ThenReturnsOK()
        {
            // Arrange
            var expectedStatusCode = System.Net.HttpStatusCode.OK;
            var stopwatch = Stopwatch.StartNew();

            var registerContent = new RegisterRequest()
            {
                Name = "New User",
                Username = "newuser",
                Password = "newuser123"
            };

            // Act
            var response = await RegisterUser(registerContent);

            TestHelper.AssertCommonResponseParts(stopwatch, response, System.Net.HttpStatusCode.OK);

            var token = await AuthenticateUser();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            response = await _httpClient.GetAsync("/Users");

            // Assert
            TestHelper.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);

            var responseStream = await response.Content.ReadAsStreamAsync();
            var responseContent = await JsonSerializer.DeserializeAsync<List<User>>(responseStream, _jsonSerializerOptions);

            Assert.Equal(2, responseContent.Count);

        }

        [Fact, TestPriority(6)]
        public async Task WhenCallingGetAll_ThenReturnsUnauthorized()
        {
            // Arrange
            var expectedStatusCode = System.Net.HttpStatusCode.Unauthorized;
            var expectedContent = new { message = "Unauthorized" };
            var stopwatch = Stopwatch.StartNew();

            // Act
            var response = await _httpClient.GetAsync("/Users");

            // Assert
            await TestHelper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact, TestPriority(7)]
        public async Task WhenCallingGetById_ThenReturnsUnauthorized()
        {
            // Arrange
            var expectedStatusCode = System.Net.HttpStatusCode.Unauthorized;
            var expectedContent = new { message = "Unauthorized" };
            var stopwatch = Stopwatch.StartNew();

            // Act
            var response = await _httpClient.GetAsync($"/Users/{2}");

            // Assert
            await TestHelper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact, TestPriority(8)]
        public async Task WhenCallingGetById_ThenReturnsOK()
        {
            // Arrange
            var expectedStatusCode = System.Net.HttpStatusCode.OK;
            var stopwatch = Stopwatch.StartNew();

            var expectedContent = new User()
            {
                Name = "New User",
                Username = "newuser"
            };

            // Act
            var token = await AuthenticateUser();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"/Users/2");

            // Assert
            var responseStream = await response.Content.ReadAsStreamAsync();
            var responseContent = await JsonSerializer.DeserializeAsync<User>(responseStream, _jsonSerializerOptions);

            TestHelper.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);

            Assert.Equal(expectedContent.Name, responseContent.Name);
            Assert.Equal(expectedContent.Username, responseContent.Username);

        }

        [Fact, TestPriority(9)]
        public async Task WhenCallingGetById_ThenReturnsNotFound()
        {
            // Arrange
            var expectedStatusCode = System.Net.HttpStatusCode.NotFound;
            var expectedContent = new { message = "User not found" };
            var stopwatch = Stopwatch.StartNew();

            // Act
            var token = await AuthenticateUser();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"/Users/3");

            // Assert
            await TestHelper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact, TestPriority(10)]
        public async Task WhenCallingUpdate_ThenReturnsUnauthorized()
        {
            // Arrange
            var expectedStatusCode = System.Net.HttpStatusCode.Unauthorized;
            var expectedContent = new { message = "Unauthorized" };
            var stopwatch = Stopwatch.StartNew();

            var content = new UpdateRequest()
            {
                Name = "New User Updated",
                Username = "newuser",
                Password = "newuser123"
            };

            // Act
            var response = await _httpClient.PutAsync($"/Users/2", TestHelper.GetJsonStringContent(content));

            // Assert
            await TestHelper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact, TestPriority(11)]
        public async Task WhenCallingUpdate_ThenReturnsNotFound()
        {
            // Arrange
            var expectedStatusCode = System.Net.HttpStatusCode.NotFound;
            var expectedContent = new { message = "User not found" };
            var stopwatch = Stopwatch.StartNew();

            var content = new UpdateRequest()
            {
                Name = "New User Updated",
                Username = "newuser",
                Password = "newuser123"
            };

            // Act
            var token = await AuthenticateUser();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PutAsync($"/Users/0", TestHelper.GetJsonStringContent(content));

            // Assert
            await TestHelper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact, TestPriority(12)]
        public async Task WhenCallingUpdate_ThenReturnsOk()
        {
            // Arrange
            var expectedStatusCode = System.Net.HttpStatusCode.OK;
            var expectedContent = new { message = "User updated successfully" };
            var stopwatch = Stopwatch.StartNew();

            var content = new UpdateRequest()
            {
                Name = "New User Updated",
                Username = "newuser",
                Password = "newuser123"
            };

            // Act
            var token = await AuthenticateUser();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PutAsync($"/Users/2", TestHelper.GetJsonStringContent(content));

            // Assert
            await TestHelper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);

            response = await _httpClient.GetAsync($"/Users/2");

            var responseStream = await response.Content.ReadAsStreamAsync();
            var responseContent = await JsonSerializer.DeserializeAsync<User>(responseStream, _jsonSerializerOptions);

            TestHelper.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);

            Assert.Equal("New User Updated", responseContent.Name);
            Assert.Equal("newuser", responseContent.Username);
        }

        [Fact, TestPriority(13)]
        public async Task WhenCallingDelete_ThenReturnsUnauthorized()
        {
            // Arrange.
            var expectedStatusCode = System.Net.HttpStatusCode.Unauthorized;
            var expectedContent = new { message = "Unauthorized" };
            var stopwatch = Stopwatch.StartNew();

            // Act.
            var response = await _httpClient.DeleteAsync($"/Users/2");

            // Assert.
            await TestHelper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact, TestPriority(14)]
        public async Task WhenCallingDelete_ThenReturnsNotFound()
        {
            // Arrange.
            var expectedStatusCode = System.Net.HttpStatusCode.NotFound;
            var expectedContent = new { message = "User not found" };
            var stopwatch = Stopwatch.StartNew();

            // Act.
            var token = await AuthenticateUser();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"/Users/0");

            // Assert.
            await TestHelper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact, TestPriority(15)]
        public async Task WhenCallingDelete_ThenReturnsOK()
        {
            // Arrange.
            var expectedStatusCode = System.Net.HttpStatusCode.OK;
            var expectedContent = new { message = "User deleted successfully" };
            var stopwatch = Stopwatch.StartNew();

            // Act.
            var token = await AuthenticateUser();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"/Users/2");

            // Assert.
            await TestHelper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }





        private async Task<HttpResponseMessage> RegisterUser(RegisterRequest? content = null)
        {
            if (content == null)
            {
                content = new RegisterRequest()
                {
                    Name = "Test User",
                    Username = "testuser",
                    Password = "testuser123"
                };
            }

            var response = await _httpClient.PostAsync("/Users/register", TestHelper.GetJsonStringContent(content));

            return response;
        }

        private async Task<string> AuthenticateUser()
        {
            var content = new AuthenticateRequest()
            {
                Username = "testuser",
                Password = "testuser123"
            };

            var response = await _httpClient.PostAsync("/Users/authenticate", TestHelper.GetJsonStringContent(content));

            var responseStream = await response.Content.ReadAsStreamAsync();
            var responseContent = await JsonSerializer.DeserializeAsync<AuthenticateResponse>(responseStream, _jsonSerializerOptions);

            return responseContent.Token;
        }

    }

}