using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace TypeChatSharp;

public class AzureOpenAiLanguageModel : ILanguageModel
{
    private readonly string endpoint;
    private readonly string key;

    public int? RetryMaxAttempts { get; set; }

    public int? RetryPauseMs { get; set; }

    public async Task<string> Complete(string prompt, CancellationToken cancellationToken)
    {
        int retryCount = 0;
        
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("api-key", key);

        while (true)
        {
            var content = new
                {
                    messages = new dynamic[]
                    {
                        new
                        {
                            role = "user",
                            content = prompt
                        }
                    },
                    temperature = 0,
                    n = 1
                };
            var json = JsonSerializer.Serialize(content, options: new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var response = await httpClient.PostAsync(endpoint, new StringContent(json, Encoding.UTF8, "application/json"), cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                using var jsonDocument = await JsonDocument.ParseAsync(contentStream, cancellationToken: cancellationToken);
                if (jsonDocument.RootElement.GetProperty("choices").EnumerateArray().Any())
                {
                    var message = jsonDocument.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
                    if (!string.IsNullOrEmpty(message))
                    {
                        return message;
                    }
                }
            }

            if (retryCount >= (RetryMaxAttempts ?? 3))
            {
                throw new Exception($"REST API error ${response.StatusCode}: ${response.RequestMessage}");
            }

            await Task.Delay(RetryPauseMs ?? 1000, cancellationToken);
            retryCount++;
        }
    }

    public AzureOpenAiLanguageModel(string endpoint, string key)
    {
        this.endpoint = endpoint;
        this.key = key;
    }
}
