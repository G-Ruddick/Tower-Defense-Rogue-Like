using System.Collections.Generic;

public partial class AbilityStats {
	// string is the name of the Ability, 
	// buyPrice is the cost to buy the ability from the shop
	public static Dictionary<string, (int index, int buyPrice)> AbilityDictionary = new Dictionary<string, (int index, int buyPrice)> {
		{ "Lightning Strike", (0, 200) }
		// { "Meteor Strike", (1, 200) }
	};

	// resetting all stats to default
	public static void ResetStats() {
		AbilityDictionary = new Dictionary<string, (int index, int buyPrice)> {
			{ "Lightning Strike", (0, 200) }
			// { "Meteor Strike", (1, 200) }
		};
	}
}
