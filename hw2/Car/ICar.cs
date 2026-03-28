namespace hw2;

public interface ICar
{
  string ModelName { get; }
  string Description { get; }
  int Seats { get; }
  int RangeKm { get; }
  IReadOnlyList<string> Features { get; }
  IEngine Engine { get; }
  IGearbox Gearbox { get; }
}
