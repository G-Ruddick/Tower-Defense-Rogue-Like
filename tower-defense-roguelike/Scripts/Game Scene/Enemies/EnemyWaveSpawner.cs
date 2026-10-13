using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class EnemyWaveSpawner : Node {
	private List<string> normalTroops = new List<string>();
	private List<string> supportTroops = new List<string>();
	private List<string> miniBossTroops = new List<string>();

	public static EnemyWaveSpawner instance;
	
	public override void _Ready() {
		instance = this;
	}

	private void ResetTroopLists() {
		normalTroops = new List<string>();
		supportTroops = new List<string>();
		miniBossTroops = new List<string>();
	}

	public List<string> ExportList() {
		GetEnemies();

		List<string> allTroops = normalTroops;

		foreach (string name in supportTroops) {
			allTroops.Add(name);
		}
		foreach (string name in miniBossTroops) {
			allTroops.Add(name);
		}

		return allTroops;
	}

	private void GetEnemies() {
		ResetTroopLists();

		// currentMoney = start value + multiplier * wave^2
		int currentMoney = GameManager.waveCostStart + GameManager.waveCostMultiplier * (int)Mathf.Pow(GameManager.gameManager.waveNumber, 2);

		int troopBudget = currentMoney / ((int)(GD.Randi() % 7) + 3);
		currentMoney -= troopBudget;
		GetTroops(ref normalTroops, ref troopBudget, EnemyStats.GrasslandEnemies, EnemyStats.enemyTypes.normal);
		
		if (currentMoney <= 0) { return; }
		troopBudget = currentMoney / ((int)(GD.Randi() % 5) + 2);
		currentMoney -= troopBudget;
		GetTroops(ref supportTroops, ref troopBudget, EnemyStats.GrasslandEnemies, EnemyStats.enemyTypes.support);

		if (currentMoney <= 0) { return; }
		GetTroops(ref miniBossTroops, ref currentMoney, EnemyStats.GrasslandEnemies, EnemyStats.enemyTypes.miniBoss);
	
		if (currentMoney <= 0) { return; }
		GetTroops(ref normalTroops, ref currentMoney, EnemyStats.GrasslandEnemies, EnemyStats.enemyTypes.normal);

		// PrintTroops();
	}

	private void GetTroops(ref List<string> troopList, ref int budget, Dictionary<string, (int cost, EnemyStats.enemyTypes type)> biomeList, EnemyStats.enemyTypes type) {
		Dictionary<string, int> enemies = EnemyStats.GetEnemiesOfType(biomeList, type);
		while (budget > 0 && enemies.Count != 0) {
			int index = (int)GD.Randi() % enemies.Count;

			if (enemies.ElementAt(index).Value <= budget) {
				budget -= enemies.ElementAt(index).Value;
				troopList.Add(enemies.ElementAt(index).Key);
			}
			else {
				enemies.Remove(enemies.ElementAt(index).Key);
			}
		}
	}

	private void PrintTroops() {
		GD.Print("Normal Troops:");
		for (int i = 0; i < normalTroops.Count; i++) {
			GD.Print("\t" + normalTroops[i]);
		}

		GD.Print("Support Troops:");
		for (int i = 0; i < supportTroops.Count; i++) {
			GD.Print("\t" + supportTroops[i]);
		}

		GD.Print("Minibosses:");
		for (int i = 0; i < miniBossTroops.Count; i++) {
			GD.Print("\t" + miniBossTroops[i]);
		}
	}
}
