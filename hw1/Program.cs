using hw1.Parser;
using hw1.Tokenizer;

ITokenizer tokenizer;

while (true)
{
  try
  {
    var maybeTokenizer = ConsoleInputTokenizer.ReadFromConsole();
    if (maybeTokenizer == null)
    {
      break;
    }

    tokenizer = maybeTokenizer;
  }
  catch (UnterminatedExpressionException e)
  {
    Console.WriteLine("Invalid expression!");
    Console.WriteLine(e.Message);
    Environment.Exit(1);
    break;
  }

  IParser parser = new RecursiveDescentParser(tokenizer);

  try
  {
    float result = parser.ComputeExpression();
    Console.WriteLine(result);
  }
  catch (DivisionByZeroException e)
  {
    Console.WriteLine("Error when computing expression!");
    Console.WriteLine(e.Message);
  }
  catch (Exception e) when (
    e is InvalidCharacterException ||
    e is InvalidNumberException ||
    e is ExpressionEndedException ||
    e is UnexpectedTokenException
  )
  {
    Console.WriteLine("Invalid expression!");
    Console.WriteLine(e.Message);
  }
}
