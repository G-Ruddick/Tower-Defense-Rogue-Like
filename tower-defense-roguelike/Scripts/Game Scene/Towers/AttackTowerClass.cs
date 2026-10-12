using Godot;
using System;
using System.Collections.Generic;

public partial class AttackTowerClass : Node3D {
	private int enemyLayer = 1 << 2;
	private List<EnemyClass> enemyTargets = new List<EnemyClass>();

	private List<ProjectileClass> projectilePool = new List<ProjectileClass>();

	[ExportCategory("Variables")]
	[Export] public int numberOfAttacks;
	[Export] public int damage;
	[Export] public float projectileSpeed;
	[Export] public float reloadTime;
	[Export] public int projectilePoolSize;
	[Export] public bool enabled;

	[ExportCategory("Components")]
	[Export] public Area3D attackArea;
	[Export] public CollisionShape3D attackRange;
	[Export] public Node3D projectile;
	[Export] public Node parentProjectile;
	[Export] private Node3D[] projectileStartingPositions;

	[ExportCategory("UI")]
	[Export] public Sprite3D rangeSprite;

	public override void _Ready() {
		attackArea.AreaEntered += OnRangeEnter;
		attackArea.AreaExited += OnRangeExit;
		projectile.Visible = false;

		Attack();
	}

	public override void _Process(double delta) {
		for (int i = projectilePool.Count - 1; i >= 0; i--) {
			if (i > projectilePoolSize && projectilePool[i].Visible == false) {
				projectilePool[i].QueueFree();
			}
		}
	}

	private void OnRangeEnter(Node3D body) {
		if (body is Area3D enemyLayer) {
			// GD.Print("enemy entered");

			enemyTargets.Add(body.GetParent() as EnemyClass);
		}
	}
	
	private void OnRangeExit(Node3D body) {
		if (body is Area3D enemyLayer) {
			// GD.Print("enemy exited");

			for (int i = 0; i < enemyTargets.Count; i++) {
				if (enemyTargets[i] == body.GetParent() as EnemyClass) {
					enemyTargets.RemoveAt(i);
					break;
				}
			}

		}
	}

	private async void Attack() {
		while (true) {
			if (enemyTargets.Count == 0) {
				await ToSignal(GetTree().CreateTimer(0), "timeout");
				continue; 
			}

			int overflow = 0;
			for(int i = 0; i < numberOfAttacks; i++) {
				bool fired = false;
				
				if (i >= enemyTargets.Count) {
					overflow++;
				}

				for (int j = 0; j < projectilePool.Count; j++) {
					if (projectilePool[j].Visible == false) {
						projectilePool[j].FireProjectile(damage, projectileSpeed, projectileStartingPositions[i].Position, enemyTargets[i - overflow]);
						fired = true;
						break;
					}
				}

				if (!fired) {  
					ProjectileClass newProjectile = (ProjectileClass)projectile.Duplicate();
					projectilePool.Add(newProjectile);
					parentProjectile.AddChild(newProjectile);
					newProjectile.FireProjectile(damage, projectileSpeed, projectileStartingPositions[i].Position, enemyTargets[i - overflow]);
				}
			}

			await ToSignal(GetTree().CreateTimer(reloadTime), "timeout");
		}
	}
}
