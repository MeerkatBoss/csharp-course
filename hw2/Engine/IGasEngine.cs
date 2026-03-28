namespace hw2;

public interface IGasEngine : IEngine
{
  double Displacement { get; }
  int Cylinders { get; }
  int TankCapacity { get; }
}