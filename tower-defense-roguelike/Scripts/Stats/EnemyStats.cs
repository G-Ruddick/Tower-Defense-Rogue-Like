using System.Collections.Generic;

public partial class EnemyStats {
	public static enum enemyTypes { normal, support, miniBoss, boss }

	public static Dictionary<string, (int cost, enemyTypes type)> GrasslandEnemies = new Dictionary<string, (int cost, enemyTypes type)> {
		{ 
			"Goblin", (1, enemyTypes.normal),
			"Mage", (5, enemyTypes.support),
			"Dark Knight", (20, enemyTypes.miniBoss)
		}
	};

	public static Dictionary<string, int> GetEnemysOfType(Dictionary<string, (int cost, enemyTypes type)> dictionary, enemyTypes type) {
		Dictionary<string, int> enemies = new Dictionary<string, int>();
		
		for (int i = 0; i < dictionary.Count; i++) {
			if (dictionary[i].type == type) {
				enemies.Add(dictionary[i], dictionary[i].cost);
			}
		}

		return enemies;
	}
}
