namespace hw2;

public class ElectricEngine(
  int horsepower,
  int torque,
  int batteryCapacity
  ) : AEngine(horsepower, torque), IEngine, IElectricEngine
{
  public int BatteryCapacity { get; } = batteryCapacity;

  public override string Description =>
    $"""
    {base.Description}
    Battery Capacity: {BatteryCapacity} kWh
    """;
}