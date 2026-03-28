namespace hw2;

public class Gazelle() : ACar(
  "GAZ-330252-244",
  3,
  870,
  [
    "4.1x1.9x0,4m cargo space",
    "1.5t max payload"
  ],
  new GasEngine(100, 220, 2.89, 4, 64),
  new ManualGearbox(5)
  )
{

}