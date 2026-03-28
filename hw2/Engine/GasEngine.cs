namespace hw2;

public class GasEngine(
  int horsepower,
  int torque,
  double displacement,
  int cylinders,
  int tankCapacity
  ) : AEngine(horsepower, torque), IEngine, IGasEngine
{
  public double Displacement { get; } = displacement;
  public int Cylinders { get; } = cylinders;
  public int TankCapacity { get; } = tankCapacity;

  public override string Description =>
    $"""
    {base.Description}
    Displacement: {Displacement} L
    Cylinders: {Cylinders}
    Tank Capacity: {TankCapacity} L
    """;
}