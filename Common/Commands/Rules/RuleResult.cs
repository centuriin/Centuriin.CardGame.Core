namespace Centuriin.CardGame.Core.Common.Commands.Rules;

public readonly record struct RuleResult
{
    public bool IsSuccess { get; }

    public string? ErrorMessage { get; }

    private RuleResult(bool isSucces, string message) : this(isSucces)
    {
        ErrorMessage = message;
    }

    private RuleResult(bool isSucces)
    {
        IsSuccess = isSucces;
    }

    public static RuleResult Success() => new(true);

    public static RuleResult Failure(string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        return new(false, message);
    }
}