using Godot;
using System;

public partial class AttackTowerClass : Node3D {
	private int enemyLayer = 1 << 2;
	private EnemyClass[] enemyTargets;

	[ExportCategory("Variables")]
	[Export] public int numberOfAttacks;
	[Export] public int Damage;
	[Export] public float projectileSpeed;
	[Export] public float reloadTime;
	[Export] public bool enabled;

	[ExportCategory("Components")]
	[Export] public Area3D attackArea;
	[Export] public CollisionShape3D attackRange;
	[Export] public Node3D projectile;

	[ExportCategory("UI")]
	[Export] public Sprite3D rangeSprite;

	public override void _Ready() {
		enemyTargets = new EnemyClass[numberOfAttacks];

		attackArea.AreaEntered += OnRangeEnter;
		attackArea.AreaExited += OnRangeExit;

		Attack();
	}

	private void OnRangeEnter(Node3D body) {
		if (body is Area3D enemyLayer) {
			GD.Print("enemy entered");

			for (int i = 0; i < enemyTargets.Length; i++) {
				if (enemyTargets[i] == null) {
					enemyTargets[i] = body.GetParent() as EnemyClass;
					break;
				}
			}
		}
	}
	
	private void OnRangeExit(Node3D body) {
		if (body is Area3D enemyLayer) {
			GD.Print("enemy exited");

			for (int i = 0; i < enemyTargets.Length; i++) {
				if (enemyTargets[i] == body.GetParent() as EnemyClass) {
					enemyTargets[i] = null;
					break;
				}
			}
		}
	}

	private async void Attack() {
		while (true) {
			for(int i = 0; i < enemyTargets.Length; i++) {
				if (enemyTargets[i] != null) {
					GD.Print("Attacking");
					enemyTargets[i].TakeDamage(Damage);
				}
			}

			await ToSignal(GetTree().CreateTimer(reloadTime), "timeout");
		}
	}
}
