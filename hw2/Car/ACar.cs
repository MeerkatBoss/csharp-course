using System.Text;

namespace hw2;

public abstract class ACar(
  string modelName,
  int seats,
  int rangeKm,
  IReadOnlyList<string> features,
  IEngine engine,
  IGearbox gearbox
  ) : ICar
{
  public string ModelName { get; } = modelName;
  public int Seats { get; } = seats;
  public int RangeKm { get; } = rangeKm;
  public IReadOnlyList<string> Features { get; } = features.ToList().AsReadOnly();
  public IEngine Engine { get; } = engine;
  public IGearbox Gearbox { get; } = gearbox;

  public virtual string Description =>
    $"""
    {ModelName} with {Seats} seats and range of {RangeKm} km.
    Engine details:
    {Indent(1, Engine.Description)}
    Gearbox details:
    {Indent(1, Gearbox.Description)}
    Additional features:
    {Indent(1, string.Join("\n", Features))}
    """;

  static protected string Indent(int level, string text)
  {
    StringBuilder result = new();
    string[] lines = text.Split(["\n", "\r\n"], StringSplitOptions.None);
    foreach (var line in lines)
    {
      result.Append(new string(' ', level * 2));
      result.AppendLine(line);
    }
    return result.ToString();
  }
}