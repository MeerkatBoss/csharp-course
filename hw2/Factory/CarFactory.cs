using System.ComponentModel;

namespace hw2;

public class CarFactory : ICarFactory
{
  public ICar CreateCar(CarType type)
  {
    return type switch
    {
      CarType.Regular => new Nissan(),
      CarType.Electric => new Tesla(),
      CarType.Cargo => new Gazelle(),
      _ => throw new InvalidEnumArgumentException()
    };
  }
}