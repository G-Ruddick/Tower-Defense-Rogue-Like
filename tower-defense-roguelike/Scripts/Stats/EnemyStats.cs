using System.Collections.Generic;

public static class EnemyStats {
	public enum enemyTypes { normal, support, miniBoss, boss }

	public static Dictionary<string, (int cost, enemyTypes type)> GrasslandEnemies = new Dictionary<string, (int cost, enemyTypes type)> {
		{ "Goblin", (1, enemyTypes.normal) },
		{ "Mage", (5, enemyTypes.support) },
		{ "Dark Knight", (20, enemyTypes.miniBoss) }
	};

	public static Dictionary<string, int> GetEnemiesOfType(Dictionary<string, (int cost, enemyTypes type)> dictionary, enemyTypes type) {
		Dictionary<string, int> enemies = new Dictionary<string, int>();
		
		foreach(var enemy in dictionary) {
			if (enemy.Value.type == type) {
				enemies.Add(enemy.Key, enemy.Value.cost);
			}
		}

		return enemies;
	}
}
