using Godot;

public class TowerUpgrades {
	public static void ArcherTower(TowerClass tower, int upgrade, float valueChanged) {
		if (upgrade == 1) {
			tower.attackClass.damage += (int)valueChanged;
			GD.Print("Upgraded tower damage to " + tower.attackClass.damage);
		}
	}
	
	public static void MortarTower(TowerClass tower, int upgrade, float valueChanged) {
		if (upgrade == 1) {
			ProjectileClass projectile = tower.attackClass.projectile as ProjectileClass;
			projectile.ChangeAOESize(valueChanged);
			GD.Print("Upgraded tower AOE size");
		}
	}
	
	public static void MageTower(TowerClass tower, int upgrade, float valueChanged) {
		if (upgrade == 1) {
			tower.attackClass.travelTime += valueChanged;
			GD.Print("Upgraded tower travel time to " + tower.attackClass.travelTime);
		}
	}
}
