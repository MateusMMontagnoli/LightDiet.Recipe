using System.Text;
using System.Text.Json;
using Xunit.Sdk;

namespace LightDiet.Recipe.EndToEndTests.Common;

public class ApiClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<(HttpResponseMessage?, TOutput?)> Post<TOutput>(
        string route,
        object payload
    )
        where TOutput : class
    {
        var serializedInput = JsonSerializer.Serialize(payload);

        var response = await _httpClient.PostAsync(
            route,
            CreateJsonStringContent(serializedInput)
        );

        var outputString = await response.Content.ReadAsStringAsync();

        TOutput? deserializedOutput = null;

        if (!string.IsNullOrWhiteSpace(outputString))
        {
            deserializedOutput = JsonSerializer.Deserialize<TOutput>(
                outputString,
                jsonSerializerOptions
            );
        }

        return (response, deserializedOutput);
    }

    public async Task<(HttpResponseMessage?, TOutput?)> Get<TOutput>(
        string route
    )
        where TOutput : class
    {
        var response = await _httpClient.GetAsync(route);
        var outputString = await response.Content.ReadAsStringAsync();
        
        var validOutputString = !string.IsNullOrWhiteSpace(outputString);

        TOutput? deserializedOutput = null;

        if (validOutputString)
        {
            deserializedOutput = JsonSerializer.Deserialize<TOutput>(
                outputString,
                jsonSerializerOptions
            );
        }

        return (response, deserializedOutput);
    }

    private StringContent CreateJsonStringContent(string serializedInput)
    {
        return new StringContent(
                serializedInput,
                Encoding.UTF8,
                "application/json"
        );
    }
}
