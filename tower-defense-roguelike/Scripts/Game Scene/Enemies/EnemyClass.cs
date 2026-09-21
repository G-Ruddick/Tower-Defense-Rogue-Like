using Godot;
using System;

public partial class EnemyClass : Node3D {
	[ExportCategory("Attributes")]
	[Export] private string enemyName;
	[Export] private float health;
	[Export] public float speed;
	[Export] public float attackPower;
	[Export] private int gold;
	[Export] public int lifeSteal;
	[Export] public Vector3 walkLocation;


	[ExportCategory("Components")]
	[Export] private Sprite3D enemySprite;
	[Export] public Area3D enemyRangeArea;
	[Export] public Area3D enemyHitbox;
	[Export] private RayCast3D groundLook;

	public override void _Ready() {
		groundLook.ForceRaycastUpdate();
	}

	public override void _Process(double delta) {
		this.Position = this.Position.MoveToward(walkLocation, speed / 10 * (float)delta);

		if (this.Position == walkLocation && groundLook.IsColliding()) {
			Node3D path = (groundLook.GetCollider() as Area3D).GetParent() as Node3D;
			
			if (path.Rotation.Y == 0) {
				walkLocation.Z += 0.32f;
			}
			else if (path.Rotation.Y == Mathf.DegToRad(180)) {
				walkLocation.Z -= 0.32f;
			}
			else if (path.Rotation.Y == Mathf.DegToRad(90)) {
				walkLocation.X += 0.32f;
			}
			else if (path.Rotation.Y == Mathf.DegToRad(-90)) {
				walkLocation.X -= 0.32f;
			}
		}
	}

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

	public void LifeSteal() {
		PlayerManager.instance.ChangeLives(-lifeSteal);
		GD.Print("Despawning enemy");
		this.QueueFree();
	}
}
