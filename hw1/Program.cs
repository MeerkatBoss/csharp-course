using System.Text;

// We don't know about classes yet, so global variables shall suffice

int parseIndex = 0;
uint lineNumber = 1;
uint charNumber = 1;
string text = "";

// Main loop
{
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
}

void evaluate(string expression)
{
  text = expression;

  // Without exceptions we have to use Go-style error handling, returning a pair of (success, value)
  (bool success, long value) = parseExpression();

  if (success && parseEnd())
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

bool parseEnd()
{
  skipWhitespace();
  if (hasChar())
  {
    char ch = advanceChar();
    if (ch == '=')
    {
      return true;
    }

    Console.WriteLine(
      string.Format(
        "Error at {0}: Expected '=' but found '{1}'", lastPosition(), ch
      )
    );

    return false;
  }

  Console.WriteLine(
    string.Format(
      "Error at {0}: Expression must end with '='", currentPosition()
    )
  );

  return false;
}

(bool, long) parseInt()
{
  long result = 0;

  if (!char.IsDigit(peekChar()))
  {
    Console.WriteLine(
      string.Format(
        "Error at {0}: Expected an integer but found '{1}'", currentPosition(), peekChar()
      )
    );
    return (false, 0);
  }

  while (hasChar() && char.IsDigit(peekChar()))
  {
    char ch = advanceChar();
    result = result * 10 + (ch - '0');
  }
  return (true, result);
}

(bool, long) parseExpression()
{
  (bool success, long value) = parseInt();
  if (!success)
    return (false, 0);

  while (hasChar())
  {
    skipWhitespace();
    long sign = peekChar() switch
    {
      '+' => 1,
      '-' => -1,
      _ => 0
    };

    if (sign == 0)
    {
      return (true, value);
    }

    advanceChar();
    skipWhitespace();
    (success, long nextValue) = parseInt();
    if (!success)
      return (false, 0);
    
    value += sign * nextValue;
  }

  return (true, value);
}
