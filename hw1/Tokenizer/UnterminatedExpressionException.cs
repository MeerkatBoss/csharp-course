namespace hw1.Tokenizer;

public class UnterminatedExpressionException(
  string expression
) : Exception($"Unterminated expression: {expression}")
{
    public string Expression { get; } = expression;
}
