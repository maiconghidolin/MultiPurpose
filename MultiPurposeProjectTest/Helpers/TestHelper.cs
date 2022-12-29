using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MultiPurposeProjectTest.Helpers;

public static class TestHelper
{

    public const int _expectedMaxElapsedMilliseconds = 2000;
    public const string _jsonMediaType = "application/json";
    public static readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public static async Task AssertResponseWithContentAsync<T>(Stopwatch stopwatch, HttpResponseMessage response, System.Net.HttpStatusCode expectedStatusCode, T expectedContent)
    {
        AssertCommonResponseParts(stopwatch, response, expectedStatusCode);

        var responseStream = await response.Content.ReadAsStreamAsync();
        var responseContent = await JsonSerializer.DeserializeAsync<T?>(responseStream, _jsonSerializerOptions);
        Assert.Equal(expectedContent, responseContent);
    }

    public static void AssertCommonResponseParts(Stopwatch stopwatch, HttpResponseMessage response, System.Net.HttpStatusCode expectedStatusCode)
    {
        Assert.Equal(expectedStatusCode, response.StatusCode);
        Assert.Equal(_jsonMediaType, response.Content.Headers.ContentType?.MediaType);
        //Assert.True(stopwatch.ElapsedMilliseconds < _expectedMaxElapsedMilliseconds);
    }

    public static StringContent GetJsonStringContent<T>(T model) => new(JsonSerializer.Serialize(model), Encoding.UTF8, _jsonMediaType);

}

