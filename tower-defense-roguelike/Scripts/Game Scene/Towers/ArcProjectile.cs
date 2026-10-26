using Godot;
using System;

public partial class ArcProjectile : ProjectileClass {
	[Export] private CollisionShape3D diagonalCollision;

	public override void _Process(double delta) {
		if (this.LinearVelocity.X >= 0) {
			projectileSprite.FlipH = true;
		}
		else {
			projectileSprite.FlipH = false;
		}

		if (this.LinearVelocity.Y <= 0) {
			projectileSprite.FlipV = true;
		}
		else {
			projectileSprite.FlipV = false;
		}

		if (MathF.Abs(this.LinearVelocity.X) > MathF.Abs(this.LinearVelocity.Y) || MathF.Abs(this.LinearVelocity.Z) > MathF.Abs(this.LinearVelocity.Y)) {
			projectileSprite.Play("Straight");
			diagonalCollision.Visible = false;
			baseCollision.Visible = true;
		}
		else {
			projectileSprite.Play("Diagonal");
			diagonalCollision.Visible = true;
			baseCollision.Visible = false;
		}
	}

	public override async void FireProjectile(int damage, float travelTime, Vector3 startPosition, EnemyClass target) {
		this.Visible = true;
		this.damage = damage;
		this.travelTime = travelTime;
		this.Position = startPosition;
		this.target = target;
		this.GravityScale = 1f;

		float gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity") * this.GravityScale;

		Vector3 distance = target.enemyHitbox.GlobalPosition - this.GlobalPosition;

		Vector3 velocity = distance / travelTime;
		float vy = (distance.Y + 0.5f * gravity * travelTime * travelTime) / travelTime;
		velocity = new Vector3(velocity.X, vy, velocity.Z);
		float vh = Mathf.Sqrt(distance.X * distance.X * distance.Z * distance.Z);

		this.LinearVelocity = velocity;

		await ToSignal(GetTree().CreateTimer(2.0f), "timeout");
		this.Visible = false;
		this.LinearVelocity = Vector3.Zero;
	}
}
