using System.Text;

namespace hw1.Tokenizer;

public class ConsoleInputTokenizer : ITokenizer
{
    private readonly string _input;
    private int _parseIndex;
    private int _line;
    private int _column;

    private ConsoleInputTokenizer(string input)
    {
        _input = input;
        _parseIndex = 0;
        _line = 1;
        _column = 1;
    }

    public static ConsoleInputTokenizer? ReadFromConsole()
    {
        var input = new StringBuilder();
        while (true)
        {
            var line = Console.ReadLine()?.TrimEnd();
            if (line == null)
            {
                break;
            }
            input.AppendLine(line);

            if (line.EndsWith(ExpressionTerminator))
            {
                break;
            }
        }

        string text = input.ToString().TrimEnd();

        if (text.EndsWith(ExpressionTerminator))
        {
            return new ConsoleInputTokenizer(text);
        }
        else if (text.IsWhiteSpace())
        {
            return null;
        }
        else
        {
            throw new UnterminatedExpressionException(text);
        }
    }

    public Token? GetNextToken()
    {
        while (_parseIndex < _input.Length && char.IsWhiteSpace(_input[_parseIndex]))
        {
            if (_input[_parseIndex] == NewLine)
            {
                _line++;
                _column = 1;
            }
            else
            {
                _column++;
            }
            _parseIndex++;
        }

        if (_parseIndex >= _input.Length)
        {
            return null;
        }

        TokenType tokenType = _input[_parseIndex] switch
        {
            '+' => TokenType.Plus,
            '-' => TokenType.Minus,
            '*' => TokenType.Star,
            '/' => TokenType.Slash,
            '(' => TokenType.LeftParen,
            ')' => TokenType.RightParen,
            '=' => TokenType.EndOfExpression,
            var ch when char.IsDigit(ch) => TokenType.Number,
            var ch => throw new InvalidCharacterException(ch, _line, _column)
        };

        if (tokenType != TokenType.Number)
        {
            var token = new Token(tokenType, _input.AsMemory(_parseIndex, 1), null, _line, _column);
            _parseIndex++;
            _column++;
            return token;
        }

        int startIndex = _parseIndex;
        while (_parseIndex < _input.Length && IsNumberChar(_input[_parseIndex]))
        {
            _parseIndex++;
            _column++;
        }
        int length = _parseIndex - startIndex;
        bool isValidNumber = float.TryParse(_input.AsSpan(startIndex, length), out float numericValue);

        if (!isValidNumber)
        {
            throw new InvalidNumberException(
              _input.Substring(startIndex, length),
              _line,
              _column - length
            );
        }

        return new Token(
          TokenType.Number,
          _input.AsMemory(startIndex, length),
          numericValue,
          _line,
          _column - length
        );
    }

    private static readonly char ExpressionTerminator = '=';
    private static readonly char NewLine = '\n';
    private static bool IsNumberChar(char ch) => char.IsDigit(ch) || ch == '.';
}