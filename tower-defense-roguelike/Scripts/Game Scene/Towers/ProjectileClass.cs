using Godot;
using System;

public partial class ProjectileClass : RigidBody3D {
	// private int enemyLayer = (1 << 2) | (1 << 3);
	protected int damage;
	protected float speed;
	protected EnemyClass target;

	[Export] protected AnimatedSprite3D projectileSprite;
	[Export] protected CollisionShape3D baseCollision;
	[Export] protected Area3D projectileArea;

	public override void _Ready() {
		projectileArea.AreaEntered += Hit;
	}

	public override void _Process(double delta) {
		if (this.LinearVelocity.X >= 0) {
			this.Scale = new Vector3(1, 1, 1);
		}
		else {
			this.Scale = new Vector3(-1, 1, 1);
		}
	}

	private async void Hit(Node3D body) {
		if (body is Area3D enemyArea) {
			EnemyClass enemy = enemyArea.GetParent<Node3D>() as EnemyClass;

			if (enemy == target) {
				enemy.TakeDamage(damage);
			}
		}
		else {
			this.LinearVelocity = Vector3.Zero;
			await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
		}

		this.Visible = false;
	}

	public virtual async void FireProjectile(int damage, float speed, Vector3 startPosition, EnemyClass target) {
		this.Visible = true;
		this.damage = damage;
		this.speed = speed;
		this.Position = startPosition;
		this.target = target;
		this.GravityScale = 0;

		Vector3 direction = (target.enemyHitbox.GlobalPosition - this.GlobalPosition).Normalized();
		this.LinearVelocity = direction * speed;

		await ToSignal(GetTree().CreateTimer(2.0f), "timeout");
		this.Visible = false;
		this.LinearVelocity = Vector3.Zero;
	}
}
