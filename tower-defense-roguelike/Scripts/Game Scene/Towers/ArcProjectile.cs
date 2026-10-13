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

	public override async void FireProjectile(int damage, float speed, Vector3 startPosition, EnemyClass target) {
		this.Visible = true;
		this.damage = damage;
		this.speed = speed;
		this.Position = startPosition;
		this.target = target;
		this.GravityScale = 1f;

		float gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity") * this.GravityScale;

		Vector3 displacement = Vector3.Up * (target.enemyHitbox.GlobalPosition.Y - this.GlobalPosition.Y);
		displacement.X = MathF.Sqrt(MathF.Pow(target.enemyHitbox.GlobalPosition.X - this.GlobalPosition.X, 2) + MathF.Pow(target.enemyHitbox.GlobalPosition.Z - this.GlobalPosition.Z, 2));
		float direction = MathF.Atan(((speed * speed) - MathF.Sqrt(MathF.Pow(speed, 4) - gravity * (gravity * MathF.Pow(displacement.X, 2) + 2 * displacement.Y * MathF.Pow(speed, 2)))) / (gravity * displacement.X));

		Vector3 velocity = Vector3.Zero;
		velocity.X = MathF.Cos(direction) * (target.enemyHitbox.GlobalPosition.X - this.GlobalPosition.X) / displacement.X;
		velocity.Y = MathF.Sin(direction);
		velocity.Z = MathF.Cos(direction) * (target.enemyHitbox.GlobalPosition.Z - this.GlobalPosition.Z) / displacement.X;

		this.LinearVelocity = velocity.Normalized() * speed;

		await ToSignal(GetTree().CreateTimer(2.0f), "timeout");
		this.Visible = false;
		this.LinearVelocity = Vector3.Zero;
	}
}
