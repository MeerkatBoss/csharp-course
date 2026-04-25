using hw1.Tokenizer;

namespace hw1.Parser;

public class ExpressionEndedException(
  TokenType expected
) : Exception(
  $"Unexpected end of expression, expected {expected.GetRepresentation()}"
)
{
    public TokenType Expected { get; } = expected;
}
