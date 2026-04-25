namespace hw1.Tokenizer;

public class InvalidNumberException(
  string value, int line, int column
) : Exception($"Invalid number '{value}' at line {line}, column {column}.")
{
  public string Value { get; } = value;
  public int Line { get; } = line;
  public int Column { get; } = column;
}
