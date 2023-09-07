using Microsoft.Extensions.Configuration;
using System.Reflection;
using TypeChatSharp;
using TypeChatSharp.Example.Sentiment;

var builder = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();
var configuration = builder.Build();

var model = new AzureOpenAiLanguageModel(configuration.GetSection("AZURE_OPENAI_ENDPOINT")?.Value ?? string.Empty, configuration.GetSection("AZURE_OPENAI_API_KEY")?.Value ?? string.Empty);

string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "Types.cs");
string schema = File.ReadAllText(filePath);

var translator = new JsonTranslator<SentimentResponse>(model, schema);

var inputsFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "input.txt");
var inputs = await File.ReadAllLinesAsync(inputsFile);
foreach (var input in inputs)
{
    if (string.IsNullOrWhiteSpace(input))
    {
        continue;
    }

    Console.WriteLine($"Chat message: {input}");

    var result = await translator.Translate(input, CancellationToken.None);
    Console.WriteLine($"The sentiment is {Enum.GetName(result.Sentiment)}");
}

Console.WriteLine("Finished. Press enter to exit.");
Console.ReadLine();