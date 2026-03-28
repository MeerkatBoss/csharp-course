namespace hw2;

public abstract class AGearbox(string type) : IGearbox
{
  public string Type { get; } = type;
  public virtual string Description =>
    $"""
    Type: {Type}
    """;
}