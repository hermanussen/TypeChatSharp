using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Text.Json;
using TypeChatSharp;
using TypeChatSharp.Example.Calendar;

var builder = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();
var configuration = builder.Build();

var model = new AzureOpenAiLanguageModel(configuration.GetSection("AZURE_OPENAI_ENDPOINT")?.Value ?? string.Empty, configuration.GetSection("AZURE_OPENAI_API_KEY")?.Value ?? string.Empty);

string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "Types.cs");
string schema = File.ReadAllText(filePath);

var translator = new JsonTranslator<CalendarActions>(model, schema);

var inputsFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "input.txt");
var inputs = await File.ReadAllLinesAsync(inputsFile);
foreach (var input in inputs)
{
    if (string.IsNullOrWhiteSpace(input))
    {
        continue;
    }

    Console.WriteLine($"Chat message: {input}");

    try
    {
        var result = await translator.Translate(input, CancellationToken.None);
        Console.WriteLine(JsonSerializer.Serialize(result, options: new() { WriteIndented = true }));

        if (result.Actions.Any(item => item.GetType() == typeof(TypeChatSharp.Example.Calendar.Action)))
        {
            Console.WriteLine("I didn't understand the following:");
            foreach (var action in result.Actions.Where(item => item.GetType() == typeof(TypeChatSharp.Example.Calendar.Action))) {
                Console.WriteLine(action.Text);
            }

            return;
        }
    }
    catch (TranslationException ex)
    {
        Console.Error.WriteLine($"Unable to translate: {ex.Message}");
        Console.Error.WriteLine(ex.StackTrace);
    }
}

Console.WriteLine("Finished. Press enter to exit.");
Console.ReadLine();