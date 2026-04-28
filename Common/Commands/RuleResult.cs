namespace Centuriin.CardGame.Core.Common.Commands;

public readonly record struct RuleResult(bool IsSuccess, string? ErrorMessage = null);