using MultiPurposeProject.Entities;
using MultiPurposeProject.Models.Users;
using MultiPurposeProjectTest.Helpers;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using Xunit;

namespace MultiPurposeProjectTest;

[TestCaseOrderer("MultiPurposeProjectTest.TestCaseOrdering.PriorityOrderer", "MultiPurposeProjectTest")]
public class UsersControllerTest : IDisposable
{

    public static readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("https://localhost:62650")
    };
    private static int _userId;
    private static string _userToken = "";

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

        var content = new RegisterRequest() { 
            Name= "Test User",
            Username= "testuser",
            Password= "testuser123"
        };

        // Act.
        var response = await _httpClient.PostAsync("/Users/register", TestHelper.GetJsonStringContent(content));

        // Assert.
        var responseStream = await response.Content.ReadAsStreamAsync();
        var responseContent = await JsonSerializer.DeserializeAsync<User>(responseStream, _jsonSerializerOptions);

        TestHelper.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);

        Assert.Equal(expectedContent.Name, responseContent.Name);
        Assert.Equal(expectedContent.Username, responseContent.Username);

        _userId = responseContent.Id;
    }

    [Fact, TestPriority(2)]
    public async Task WhenCallingRegister_ThenReturnsBadRequest()
    {
        // Arrange.
        var expectedStatusCode = System.Net.HttpStatusCode.BadRequest;
        var expectedContent = new { message = "Username 'testuser' is already taken" };
        var stopwatch = Stopwatch.StartNew();

        var content = new RegisterRequest()
        {
            Name = "Test User",
            Username = "testuser",
            Password = "testuser123"
        };

        // Act.
        var response = await _httpClient.PostAsync("/Users/register", TestHelper.GetJsonStringContent(content));

        // Assert.
        await TestHelper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
    }
  
    [Fact, TestPriority(3)]
    public async Task WhenCallingAuthenticate_ThenReturnsBadRequest()
    {
        // Arrange.
        var expectedStatusCode = System.Net.HttpStatusCode.BadRequest;
        var stopwatch = Stopwatch.StartNew();

        var expectedContent = new { message = "Username or password is incorrect" };

        var content = new AuthenticateRequest()
        {
            Username = "testures",
            Password = "testures123"
        };

        // Act.
        var response = await _httpClient.PostAsync("/Users/authenticate", TestHelper.GetJsonStringContent(content));

        // Assert.
        await TestHelper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
    }

    [Fact, TestPriority(4)]
    public async Task WhenCallingAuthenticate_ThenReturnsOK()
    {
        // Arrange.
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

        // Act.
        var response = await _httpClient.PostAsync("/Users/authenticate", TestHelper.GetJsonStringContent(content));

        // Assert.
        var responseStream = await response.Content.ReadAsStreamAsync();
        var responseContent = await JsonSerializer.DeserializeAsync<AuthenticateResponse>(responseStream, _jsonSerializerOptions);

        TestHelper.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);

        Assert.Equal(expectedContent.Name, responseContent.Name);
        Assert.Equal(expectedContent.Username, responseContent.Username);

        _userToken = responseContent.Token;
    }

    [Fact, TestPriority(5)]
    public async Task WhenCallingGetAll_ThenReturnsOK()
    {
        // Arrange.
        var expectedStatusCode = System.Net.HttpStatusCode.OK;
        var stopwatch = Stopwatch.StartNew();

        // Act.
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userToken);
        var response = await _httpClient.GetAsync("/Users");

        // Assert.
        TestHelper.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
    }

    [Fact, TestPriority(6)]
    public async Task WhenCallingGetAll_ThenReturnsUnauthorized()
    {
        // Arrange.
        var expectedStatusCode = System.Net.HttpStatusCode.Unauthorized;
        var expectedContent = new { message = "Unauthorized" };
        var stopwatch = Stopwatch.StartNew();

        // Act.
        var response = await _httpClient.GetAsync("/Users");

        // Assert.
        await TestHelper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
    }

    [Fact, TestPriority(7)]
    public async Task WhenCallingGetById_ThenReturnsUnauthorized()
    {
        // Arrange.
        var expectedStatusCode = System.Net.HttpStatusCode.Unauthorized;
        var expectedContent = new { message = "Unauthorized" };
        var stopwatch = Stopwatch.StartNew();

        // Act.
        var response = await _httpClient.GetAsync($"/Users/{_userId}");

        // Assert.
        await TestHelper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
    }

    [Fact, TestPriority(8)]
    public async Task WhenCallingGetById_ThenReturnsOK()
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
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userToken);
        var response = await _httpClient.GetAsync($"/Users/{_userId}");

        // Assert.
        var responseStream = await response.Content.ReadAsStreamAsync();
        var responseContent = await JsonSerializer.DeserializeAsync<User>(responseStream, _jsonSerializerOptions);

        TestHelper.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);

        Assert.Equal(expectedContent.Name, responseContent.Name);
        Assert.Equal(expectedContent.Username, responseContent.Username);

    }

    [Fact, TestPriority(9)]
    public async Task WhenCallingGetById_ThenReturnsNotFound()
    {
        // Arrange.
        var expectedStatusCode = System.Net.HttpStatusCode.NotFound;
        var expectedContent = new { message = "User not found" };
        var stopwatch = Stopwatch.StartNew();

        // Act.
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userToken);
        var response = await _httpClient.GetAsync($"/Users/0");

        // Assert.
        await TestHelper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
    }

    [Fact, TestPriority(10)]
    public async Task WhenCallingUpdate_ThenReturnsUnauthorized()
    {
        // Arrange.
        var expectedStatusCode = System.Net.HttpStatusCode.Unauthorized;
        var expectedContent = new { message = "Unauthorized" };
        var stopwatch = Stopwatch.StartNew();

        var content = new UpdateRequest()
        {
            Name = "Test User Updated",
            Username = "testuser",
            Password = "testuser123"
        };

        // Act.
        var response = await _httpClient.PutAsync($"/Users/{_userId}", TestHelper.GetJsonStringContent(content));

        // Assert.
        await TestHelper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
    }

    [Fact, TestPriority(11)]
    public async Task WhenCallingUpdate_ThenReturnsNotFound()
    {
        // Arrange.
        var expectedStatusCode = System.Net.HttpStatusCode.NotFound;
        var expectedContent = new { message = "User not found" };
        var stopwatch = Stopwatch.StartNew();

        var content = new UpdateRequest()
        {
            Name = "Test User Updated",
            Username = "testuser",
            Password = "testuser123"
        };

        // Act.
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userToken);
        var response = await _httpClient.PutAsync($"/Users/0", TestHelper.GetJsonStringContent(content));

        // Assert.
        await TestHelper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
    }

    [Fact, TestPriority(12)]
    public async Task WhenCallingUpdate_ThenReturnsOk()
    {
        // Arrange.
        var expectedStatusCode = System.Net.HttpStatusCode.OK;
        var expectedContent = new { message = "User updated successfully" };
        var stopwatch = Stopwatch.StartNew();

        var content = new UpdateRequest()
        {
            Name = "Test User Updated",
            Username = "testuser",
            Password = "testuser123"
        };

        // Act.
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userToken);
        var response = await _httpClient.PutAsync($"/Users/{_userId}", TestHelper.GetJsonStringContent(content));

        // Assert.
        await TestHelper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);


        response = await _httpClient.GetAsync($"/Users/{_userId}");

        var responseStream = await response.Content.ReadAsStreamAsync();
        var responseContent = await JsonSerializer.DeserializeAsync<User>(responseStream, _jsonSerializerOptions);

        TestHelper.AssertCommonResponseParts(stopwatch, response, expectedStatusCode);

        Assert.Equal("Test User Updated", responseContent.Name);
        Assert.Equal("testuser", responseContent.Username);
    }

    [Fact, TestPriority(13)]
    public async Task WhenCallingDelete_ThenReturnsUnauthorized()
    {
        // Arrange.
        var expectedStatusCode = System.Net.HttpStatusCode.Unauthorized;
        var expectedContent = new { message = "Unauthorized" };
        var stopwatch = Stopwatch.StartNew();

        // Act.
        var response = await _httpClient.DeleteAsync($"/Users/{_userId}");

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
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userToken);

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
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userToken);

        var response = await _httpClient.DeleteAsync($"/Users/{_userId}");

        // Assert.
        await TestHelper.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
    }
    

    public void Dispose()
    {
        //_httpClient.DeleteAsync("/state").GetAwaiter().GetResult();
        _httpClient.CancelPendingRequests();
        _httpClient.Dispose();
    }

}
