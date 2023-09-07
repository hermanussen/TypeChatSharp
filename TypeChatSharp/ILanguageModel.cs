namespace TypeChatSharp;

public interface ILanguageModel
{
    /// <summary>
    /// Optional property that specifies the maximum number of retry attempts (the default is 3).
    /// </summary>
    int? RetryMaxAttempts { get; }

    /// <summary>
    /// Optional property that specifies the delay before retrying in milliseconds (the default is 1000ms).
    /// </summary>
    int? RetryPauseMs { get; }

    /// <summary>
    /// Obtains a completion from the language model for the given prompt.
    /// </summary>
    /// <param name="prompt">The prompt.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string> Complete(string prompt, CancellationToken cancellationToken);
}
