namespace hw1.Tokenizer;

public readonly struct Token
{
    public TokenType Type { get; }
    public ReadOnlyMemory<char> Value { get; }
    public float? NumericValue { get; }
    public int Line { get; }
    public int Column { get; }

    internal Token(TokenType type, ReadOnlyMemory<char> value, float? numericValue, int line, int column)
    {
        Type = type;
        Value = value;
        NumericValue = numericValue;
        Line = line;
        Column = column;
    }
}
