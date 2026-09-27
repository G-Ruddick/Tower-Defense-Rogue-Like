using System.Collections.Generic;

public partial class TowerStats {
	// string is the name of the tower, 
	// buyPrice is the cost to buy the tower from the shop,
	// usePrice is the cost to place the tower on the map.
	public static Dictionary<string, (int index, int buyPrice, int usePrice)> TowerDictionary = new Dictionary<string, (int index, int buyPrice, int usePrice)> {
		{ "Archer Tower", (0, 100, 50) }
	};

	// resetting all towers to default
	public static void ResetStats() {
		TowerDictionary = new Dictionary<string, (int index, int buyPrice, int usePrice)> {
			{ "Archer Tower", (0, 100, 50) }
		};
	}
}
