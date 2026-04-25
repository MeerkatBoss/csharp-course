using hw1.Tokenizer;

namespace hw1.Parser;

public class UnexpectedTokenException(
  Token token,
  TokenType expected
) : Exception(
  $"{token.Line}:{token.Column} - Expected {expected.GetRepresentation()} but found '{token.Value}'"
)
{
    public Token Token { get; } = token;
    public TokenType Expected { get; } = expected;
}