namespace hw2;

public class Tesla() : ACar(
  "Tesla Model S",
  5,
  628,
  ["Autopilot", "Onboard computer with 17'' LCD"],
  new ElectricEngine(1020, 482, 130),
  new AutomaticGearbox(["Reverse", "Neutral", "Drive"])
)
{

}