namespace hw1.Parser;

public class DivisionByZeroException(int line, int column)
  : Exception($"Division by zero at line {line}, column {column}")
{
    public int Line { get; } = line;
    public int Column { get; } = column;
}
