namespace hw1.Tokenizer;

public class InvalidCharacterException(
  char character, int line, int column
) : Exception($"Unexpected character '{character}' at line {line}, column {column}.")
{
    public char Character { get; } = character;
    public int Line { get; } = line;
    public int Column { get; } = column;
}
