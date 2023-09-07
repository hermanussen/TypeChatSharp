using System.Text.Json;

namespace TypeChatSharp;

public class JsonTranslator<T>
{
    private readonly ILanguageModel model;
    private readonly string schema;

    public bool AttemptRepair { get; set; } = true;

    public JsonTranslator(ILanguageModel model, string schema)
    {
        this.model = model;
        this.schema = schema;
    }

    public async Task<T> Translate(string request, CancellationToken cancellationToken)
    {
        var prompt = CreateRequestPrompt(request);
        while (true)
        {
            var response = await model.Complete(prompt, cancellationToken);
            int startIndex = response.IndexOf('{');
            int endIndex = response.LastIndexOf('}');
            if(!(startIndex >= 0 && endIndex > startIndex))
            {
                throw new TranslationException($"Response is not JSON: {response}");
            }

            var jsonText = response.Substring(startIndex, endIndex - startIndex + 1);

            string validationMessage;
            try
            {
                var result = JsonSerializer.Deserialize<T>(jsonText);
                if(result != null)
                {
                    return result;
                }

                validationMessage = "Result from deserialization was null";
            }
            catch (Exception ex)
            {
                validationMessage = ex.Message;
                if (!AttemptRepair)
                {
                    throw new TranslationException($"JSON validation failed: {validationMessage}{Environment.NewLine}{response}");
                }
            }

            prompt += $"{response}{Environment.NewLine}{JsonTranslator<T>.CreateRepairPrompt(validationMessage)}";
            AttemptRepair = false;
        }
    }

    private string CreateRequestPrompt(string request)
    {
        return $@"You are a service that translates user requests into JSON objects of type ""{typeof(T).Name}"" according to the following C# definitions:
                
                {schema}

                The following is a user request:
                
                {request}

                The following is the user request translated into a JSON object with 2 spaces of indentation and no properties with the value undefined:
";
    }

    private static string CreateRepairPrompt(string validationError)
    {
        return $@"The JSON object is invalid for the following reason:
                  ""{validationError}""
                  The following is a revised JSON object:
";
    }
}
