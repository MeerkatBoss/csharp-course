using hw2;

namespace hw2;

class Nissan() : ACar(
  "Nissan Qashqai",
  7,
  750,
  ["Air Conditioning", "Bluetooth Connectivity", "Backup Camera"],
  new GasEngine(141, 196, 2.0, 4, 65),
  new AutomaticGearbox(["Reverse", "Neutral", "Drive/Sport"])
)
{

}