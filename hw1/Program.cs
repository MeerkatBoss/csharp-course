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
  (bool success, float value) = parseExpression();

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

(bool, float) parseNumber()
{
  int startIndex = parseIndex;
  while (hasChar())
  {
    char ch = peekChar();
    if (char.IsDigit(ch) || ch == '.')
    {
      advanceChar();
    }
    else
    {
      break;
    }
  }
  int endIndex = parseIndex;
  if (startIndex == endIndex)
  {
    Console.WriteLine(
      string.Format(
        "Error at {0}: Expected a number but found '{1}'", currentPosition(), peekChar()
      )
    );
    return (false, 0);
  }

  int length = endIndex - startIndex;
  var substring = text.AsSpan().Slice(startIndex, length);

  bool success = float.TryParse(substring, out float value);
  return (success, value);
}

(bool, float) parseExpression()
{
  (bool success, float value) = parseTerm();
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
    (success, float nextValue) = parseTerm();
    if (!success)
      return (false, 0);
    
    value += sign * nextValue;
  }

  return (true, value);
}

(bool, float) parseTerm()
{
  (bool success, float value) = parseGroup();
  if (!success)
    return (false, 0);
  
  while (hasChar())
  {
    skipWhitespace();
    char op = peekChar();
    string opPosition = currentPosition();

    if (op != '*' && op != '/')
    {
      return (true, value);
    }

    advanceChar();
    skipWhitespace();
    (success, float nextValue) = parseGroup();
    if (!success)
      return (false, 0);
    
    if (op == '*')
    {
      value *= nextValue;
    }
    else
    {
      if (nextValue == 0)
      {
        Console.WriteLine(
          string.Format(
            "Error at {0}: Division by zero", opPosition
          )
        );
        return (false, 0);
      }
      value /= nextValue;
    }
  }

  return (true, value);
}

(bool, float) parseGroup()
{
  skipWhitespace();
  if (peekChar() == '(')
  {
    advanceChar();
    (bool success, float value) = parseExpression();
    if (!success)
      return (false, 0);
    
    skipWhitespace();
    if (peekChar() == ')')
    {
      advanceChar();
      return (true, value);
    }

    Console.WriteLine(
      string.Format(
        "Error at {0}: Expected ')' but found '{1}'", currentPosition(), peekChar()
      )
    );
    return (false, 0);
  }

  return parseNumber();
}
