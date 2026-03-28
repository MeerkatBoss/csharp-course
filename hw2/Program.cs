using hw2;

IReadOnlyDictionary<string, CarType> cars = new Dictionary<string, CarType>
{
  ["Nissan"] = CarType.Regular,
  ["Tesla"] = CarType.Electric,
  ["Gazelle"] = CarType.Cargo
};
ICarFactory factory = new CarFactory();

string prompt = "Enter the name of the car or 'done' to finish: ";
string? input;

while (true)
{
  Console.Write(prompt);
  input = Console.ReadLine();

  if (input == null || input.ToLower() == "done")
    break;

  if (cars.TryGetValue(input, out CarType type))
  {
    ICar car = factory.CreateCar(type);
    Console.WriteLine(car.Description);
  }
  else
  {
    Console.WriteLine("Unknown car type. Please try again.");
  }
}
