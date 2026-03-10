using System.Text;

int parseIndex = 0;
uint lineNumber = 1;
uint charNumber = 1;
string text = "";

void reset()
{
  parseIndex = 0;
  lineNumber = 1;
  charNumber = 1;
  text = "";
}

string currentPosition()
{
  return string.Format("line {0}, column {1}", lineNumber, charNumber);
}

string lastPosition()
{
  if (charNumber > 1)
  {
    return string.Format("line {0}, column {1}", lineNumber, charNumber - 1);
  }
  if (lineNumber > 1)
  {
    return string.Format("line {0}, column {1}", lineNumber - 1, 1);
  }
  return "start of expression";
}

bool hasChar()
{
  return parseIndex < text.Length;
}

char peekChar()
{
  if (hasChar())
  {
    return text[parseIndex];
  }
  return '\0';
}

char advanceChar()
{
  if (hasChar())
  {
    char ch = peekChar();
    ++parseIndex;
    ++charNumber;
    return ch;
  }
  return '\0';
}

char lastChar()
{
  if (parseIndex > 0)
  {
    return text[parseIndex - 1];
  }
  return '\0';
}

long skipWhitespace()
{
  while (hasChar() && char.IsWhiteSpace(peekChar()))
  {
    char ch = advanceChar();
    if (ch == '\n')
    {
      ++lineNumber;
      charNumber = 1;
    }

  }
  return parseIndex;
}

(bool, long) parseInt()
{
  long result = 0;

  while (hasChar())
  {
    char ch = advanceChar();
    if (!char.IsDigit(ch))
    {
      Console.WriteLine(
        string.Format(
          "Error at {0}: Expected an integer but found '{1}'", lastPosition(), lastChar()
        )
      );
      return (false, 0);
    }
    result = result * 10 + (ch - '0');
  }
  return (true, result);
}

(bool, long) parseExpression()
{
  skipWhitespace();
  (bool success, long value) = parseInt();
  if (!success) return (false, 0);

  skipWhitespace();

  if (hasChar())
  {
    char ch = advanceChar();
    if (ch == '=')
    {
      return (true, value);
    }

    Console.WriteLine(
      string.Format(
        "Error at {0}: Expected '=' but found '{1}'", lastPosition(), ch
      )
    );

    return (false, 0);
  }

  Console.WriteLine(
    string.Format(
      "Error at {0}: Expression must end with '='", currentPosition()
    )
  );
  return (false, 0);
}

void evaluate(string expression)
{
  text = expression;

  (bool success, long value) = parseExpression();

  if (success)
  {
    Console.WriteLine(value);
  }

  reset();
}

bool isExpressionReady(StringBuilder builder)
{
  for (int i = builder.Length - 1; i >= 0; --i)
  {
    if (builder[i] == '=')
    {
      return true;
    }
    else if (!char.IsWhiteSpace(builder[i]))
    {
      return false;
    }
  }
  return false;
}

StringBuilder builder = new();
string? line;

while ((line = Console.ReadLine()) != null)
{
  builder.Append(line);
  builder.Append(Environment.NewLine);
  if (builder.Length == 0) continue;

  if (isExpressionReady(builder))
  {
    evaluate(builder.ToString());
    builder.Clear();
  }
}

Console.WriteLine(builder.ToString());
