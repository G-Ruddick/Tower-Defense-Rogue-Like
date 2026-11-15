using System.Collections.Generic;

public partial class AbilityStats {
	// string is the name of the Ability, 
	// buyPrice is the cost to buy the ability from the shop
	public static Dictionary<string, (int index, int buyPrice, float cooldown)> AbilityDictionary = new Dictionary<string, (int index, int buyPrice, float cooldown)> {
		{ "Lightning Strike", (0, 200, 10.0f) }
		// { "Meteor Strike", (1, 200, 15.0f) }
	};

	// resetting all stats to default
	public static void ResetStats() {
		AbilityDictionary = new Dictionary<string, (int index, int buyPrice, float cooldown)> {
			{ "Lightning Strike", (0, 200, 10.0f) }
			// { "Meteor Strike", (1, 200, 15.0f) }
		};
	}
}
