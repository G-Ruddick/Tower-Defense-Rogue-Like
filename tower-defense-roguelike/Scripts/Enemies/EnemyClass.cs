using Godot;
using System;

public partial class EnemyClass : Node {
	// enemy name
	protected string enemyName;
	public string GetEnemyName() {
		return enemyName;
	}
	
	// enemy health
	protected float enemyHealth;
	public float GetHealth() {
		return enemyHealth;
	}
	// healing enemy
	public void Heal(float HP) {
		enemyHealth += HP;
	}
	// taking damage
	public void TakeDamage(float damage) {
		enemyHealth -= damage;
	}
	// enemy death
	public void Die() {
		// playergold += enemyGold * (float)GD.RandRange(.5f, 1.25f);
		this.QueueFree();
	}

	// enemy movement speed
	protected float enemySpeed;
	public float GetSpeed() {
		return enemySpeed;
	}

	// ammount of gold enemy drops on death
	protected int enemyGold;
	public int GetGold() {
		return enemyGold;
	}

	// ammount of damage enemy does to things
	protected float enemyAttackPower;
	public float GetAttackPower() {
		return enemyAttackPower;
	}

	// enemy attack range
	protected float enemyRange;
	[Export] protected Area2D enemyRangeArea;


	[Export] protected Area2D enemyHitbox;
	[Export] protected AnimatedSprite2D enemySprite;
}
