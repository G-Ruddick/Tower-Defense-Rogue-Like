using Godot;
using System;

public partial class ProjectileClass : RigidBody3D {
	private enum HitType {SingleTarget, AOE, AOEGround}

	protected int damage;
	protected float travelTime;
	protected EnemyClass target;
	[Export] private HitType hitType;

	[Export] protected AnimatedSprite3D projectileSprite;
	[Export] protected CollisionShape3D baseCollision;
	[Export] protected Area3D projectileArea;
	[Export] protected Area3D AOEArea;


	public override void _Ready() {
		if (AOEArea != null) {
			AOEArea.Visible = false;
		}

		projectileArea.AreaEntered += Hit;

		if (AOEArea != null) {
			AOEArea.AreaEntered += AOEHit;
		}
	}

	public override void _Process(double delta) {
		if (this.LinearVelocity.X >= 0) {
			this.Scale = new Vector3(1, 1, 1);
		}
		else {
			this.Scale = new Vector3(-1, 1, 1);
		}
	}

	private void Hit(Node3D body) {
		if (hitType == HitType.SingleTarget) {
			SingleTargetDamage(body);
			return;
		}
		
		if (hitType == HitType.AOE || hitType == HitType.AOEGround) {
			AOEDamage(body);
		}
	}

	private void AOEHit(Node3D body) {
		if (body is Area3D) {
			EnemyClass enemy = body.GetParent<Node3D>() as EnemyClass;

			if (enemy != null) {
				enemy.TakeDamage(damage);
			}
		}
	}

	public virtual async void FireProjectile(int damage, float travelTime, Vector3 startPosition, EnemyClass target) {
		this.Visible = true;
		this.damage = damage;
		this.travelTime = travelTime;
		this.Position = startPosition;
		this.target = target;
		this.GravityScale = 0;

		Vector3 direction = (target.enemyHitbox.GlobalPosition - this.GlobalPosition).Normalized();
		this.LinearVelocity = direction * travelTime;

		await ToSignal(GetTree().CreateTimer(2.0f), "timeout");
		this.Visible = false;
		this.LinearVelocity = Vector3.Zero;
	}

	private async void SingleTargetDamage(Node3D body) {
		if (body is Area3D) {
			EnemyClass enemy = body.GetParent<Node3D>() as EnemyClass;

			if (enemy != null && enemy == target) {
				enemy.TakeDamage(damage);
			}
		}
		else {
			this.LinearVelocity = Vector3.Zero;
			await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
		}

		this.Visible = false;
	}
	
	private async void AOEDamage(Node3D body) {
		AOEArea.Visible = true;
		projectileArea.Visible = false;
		projectileSprite.Visible = false;
		this.LinearVelocity = Vector3.Zero;

		await ToSignal(GetTree().CreateTimer(0.01f), "timeout");
		this.Visible = false;
		AOEArea.Visible = false;
		projectileArea.Visible = true;
		projectileSprite.Visible = true;
	}
}
