using System.Text;
using System.Text.Json;

namespace LightDiet.Recipe.EndToEndTests.Common;

public class ApiClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<(HttpResponseMessage?, TOutput?)> Post<TOutput>(
        string route,
        object payload
    )
    {
        var serializedInput = JsonSerializer.Serialize(payload);

        var response = await _httpClient.PostAsync(
            route,
            new StringContent(
                serializedInput,
                Encoding.UTF8,
                "application/json"
            )
        );

        var outputString = await response.Content.ReadAsStringAsync();

        JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        var deserializedOutput = JsonSerializer.Deserialize<TOutput>(outputString,
            options
        );

        return (response, deserializedOutput);
    }
}
