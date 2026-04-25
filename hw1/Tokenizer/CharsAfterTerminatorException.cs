namespace hw1.Tokenizer;

public class CharsAfterTerminatorException(
  string chars
) : Exception(
  $"Unexpected characters after expression terminator: '{chars}'."
)
{
    public string Chars { get; } = chars;
}