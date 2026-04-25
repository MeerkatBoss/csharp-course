namespace hw1.Tokenizer;

public enum TokenType
{
  Number,
  Plus,
  Minus,
  Star,
  Slash,
  LeftParen,
  RightParen,
  EndOfExpression,
}

public static class TokenTypeExtensions
{
  public static string GetRepresentation(this TokenType tokenType) => tokenType switch
  {
    TokenType.Number => "a number",
    TokenType.Plus => "'+'",
    TokenType.Minus => "'-'",
    TokenType.Star => "'*'",
    TokenType.Slash => "'/'",
    TokenType.LeftParen => "'('",
    TokenType.RightParen => "')'",
    TokenType.EndOfExpression => "end of expression",
    _ => throw new ArgumentOutOfRangeException(nameof(tokenType), tokenType, null)
  };
}
