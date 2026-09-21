using System.Collections.Generic;

public partial class TowerStats {
	// string is the name of the tower, 
	// buyPrice is the cost to buy the tower from the shop,
	// usePrice is the cost to place the tower on the map.
	public static Dictionary<string, (int buyPrice, int usePrice)> TowerDictionary = new Dictionary<string, (int buyPrice, int usePrice)> {
		{ "Archer Tower", (100, 50) }
	};

	public void ResetStats() {
		TowerDictionary = new Dictionary<string, (int buyPrice, int usePrice)> {
			{ "Archer Tower", (100, 50) }
		};
	}
}
