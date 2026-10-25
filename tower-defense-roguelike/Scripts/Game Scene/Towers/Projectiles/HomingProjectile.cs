using Godot;
using System;

public partial class HomingProjectile : ProjectileClass {
	public override void _Process(double delta) {
		if (this.LinearVelocity.X < this.LinearVelocity.Z) {
			projectileSprite.RotationDegrees = new Vector3(-35, 0, 0);
		}
		else {
			projectileSprite.RotationDegrees = new Vector3(-35, 0, -90);
		}

		if (this.LinearVelocity.X >= 0) {
			projectileSprite.FlipH = true;
		}
		else {
			projectileSprite.FlipH = false;
		}

		if (this.LinearVelocity.Z <= 0) {
			projectileSprite.FlipV = true;
		}
		else {
			projectileSprite.FlipV = false;
		}
	}

	public override async void FireProjectile(int damage, float travelTime, Vector3 startPosition, EnemyClass target) {
		this.Visible = true;
		this.damage = damage;
		this.travelTime = travelTime;
		this.Position = startPosition;
		this.target = target;
		
		Vector3 distance = Vector3.One;
		while (true) {
			if (!this.Visible || target.enemyHitbox == null) {
				break;
			}

			distance = target.enemyHitbox.GlobalPosition - this.GlobalPosition;
			this.LinearVelocity = distance.Normalized() * travelTime;

			await ToSignal(GetTree().CreateTimer(0), "timeout");
		}
		
		this.LinearVelocity *= 0;
		await ToSignal(GetTree().CreateTimer(0), "timeout");
		this.Visible = false;
	}
}
