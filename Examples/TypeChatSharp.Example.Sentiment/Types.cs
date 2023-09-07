namespace TypeChatSharp.Example.Sentiment;

public class SentimentResponse
{
    public Sentiment Sentiment { get; set; }
}

public enum Sentiment
{
    Negative = 0,
    Neutral = 1,
    Positive = 2
}