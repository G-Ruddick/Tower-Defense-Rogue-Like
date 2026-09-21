using Godot;
using System;

public partial class EnemyClass : Node3D {
	[ExportCategory("Attributes")]
	[Export] private string enemyName;
	[Export] private float health;
	[Export] public float speed;
	[Export] public float attackPower;
	[Export] private int gold;
	[Export] private int lifeSteal;

	[ExportCategory("Components")]
	[Export] private Sprite3D enemySprite;
	[Export] public Area3D enemyRangeArea;
	[Export] public Area3D enemyHitbox;

	public string GetEnemyName() {
		return enemyName;
	}
	
	public float GetHealth() {
		return health;
	}
	public void TakeDamage(float damage) {
		health -= damage;
	}
	// enemy death
	public void Die() {
		PlayerManager.instance.ChangeMoney((int)(gold * (float)GD.RandRange(.5f, 1.25f)));
		this.QueueFree();
	}
}
