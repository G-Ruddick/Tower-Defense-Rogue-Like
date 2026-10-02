using Godot;
using System;

public partial class ProjectileClass : RigidBody3D {
	private int enemyLayer = (1 << 2) | (1 << 3);
	protected int damage;
	protected float speed;

	[Export] protected AnimatedSprite3D projectileSprite;
	[Export] protected CollisionShape3D baseCollision;
	[Export] protected Area3D projectileArea;

	public override void _Ready() {
		projectileArea.AreaEntered += Hit;
	}

	private async void Hit(Node3D body) {
		GD.Print("collided");

		if (body is Area3D enemyArea) {
			EnemyClass enemy = enemyArea.GetParent<EnemyClass>();
			enemy.TakeDamage(damage);
		}
		else {
			this.LinearVelocity = Vector3.Zero;
			await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
		}

		this.Visible = false;
	}

	public virtual async void FireProjectile(int damage, float speed, Vector3 startPosition, Vector3 target) {
		this.Visible = true;
		this.damage = damage;
		this.speed = speed;
		this.Position = startPosition;
		this.GravityScale = 0;

		Vector3 direction = (target - GlobalPosition).Normalized();
		this.LinearVelocity = direction * speed;

		await ToSignal(GetTree().CreateTimer(2.0f), "timeout");
		this.Visible = false;
		this.LinearVelocity = Vector3.Zero;
	}
}
