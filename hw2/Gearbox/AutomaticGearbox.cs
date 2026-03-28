namespace hw2;

public class AutomaticGearbox(string[] modes)
  : AGearbox("Automatic"), IAutomaticGearbox
{
  public string[] Modes { get; } = modes;
  public override string Description =>
    $"""
    {base.Description}
    Modes: {string.Join(", ", Modes)}
    """;
}