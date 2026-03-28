namespace hw2;

public abstract class AEngine(int horsepower, int torque) : IEngine
{
  public int Horsepower { get; } = horsepower;
  public int Torque { get; } = torque;
  public virtual string Description =>
    $"""
    Power: {Horsepower} HP
    Torque: {Torque} Nm
    """;
}