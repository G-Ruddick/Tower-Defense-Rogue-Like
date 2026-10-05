using Godot;
using System.Collections.Generic;

public partial class EnemyWaveSpawner : Node {
	private List<string> normalTroops = new List<string>();
	private List<string> supportTroops = new List<string>();
	private List<string> miniBossTroops = new List<string>();

	public void GetEnemies() {
		int currentMoney = GameManager.waveCost;

		int troopBudget = currentMoney / (int)(GD.Randi() % 3) + 1;
		currentMoney -= troopBudget;
		Dictionary<string, (int cost, enemyTypes type)> enemies = EnemyStats.GetEnemiesOfType(EnemyStats.GrasslandEnemies, EnemyStats.enemyTypes.normal);
		while (troopBudget > 0) {
			int index = (int)GD.Randi() % enemies.Count;
			if (enemies.ElementAt(index).Value <= troopBudget) {
				normalTroops.Add(enemies.ElementAt(index).Keys);
				
			}
			else {
				enemies.Remove(enemies.Keys.ElementAt(index));
			}
		}
	}
}
