using Godot;
using System;

public partial class ProjectileClass : Node3D {
	protected Vector3 target;
	protected int damage;
	protected float speed;

	[Export] protected AnimatedSprite3D projectileSprite;
	[Export] protected Area3D projectileArea;
	[Export] protected CollisionShape3D baseCollision; 

	// public ProjectileClass(Vector3 target, int damage, float speed) {
	// 	this.target = target;
	// 	this.damage = damage;
	// 	this.speed = speed;
	// }

	// public override void _Process(double delta) {
		
	// }

	// public virtual void FireProjectile() {
	// 	Vector3 direction = (target - GlobalPosition).Normalize();
	// 	float distance = GlobalPosition.DistanceTo(target);

	// 	Velocity = direction * speed;
	// }
}
