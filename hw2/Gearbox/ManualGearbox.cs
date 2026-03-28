namespace hw2;

public class ManualGearbox(int numberOfGears)
  : AGearbox("Manual"), IManualGearbox
{
  public int NumberOfGears { get; } = numberOfGears;
  public override string Description =>
    $"""
    {base.Description}
    Number of Gears: {NumberOfGears}
    """;
}