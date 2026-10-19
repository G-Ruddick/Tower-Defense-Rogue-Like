using Godot;
using System;

public partial class GridTile : Node3D {
	[Export] private Sprite3D tileSpite;
	[Export] private RayCast3D tileChecker;

	public override void _Ready() {
		tileChecker.ForceRaycastUpdate();
	}

	public void ChangeColor(bool validPlacement) {
		if (validPlacement) {
			tileSpite.Modulate = new Color(1, 1, 1, 0.75f);
			return;
		}

		tileSpite.Modulate = new Color(10f, 0, 0, 1);
	}

	public void SetVisibility(bool visibility) {
		this.Visible = visibility;
	}

	public void CheckForObjects() {
		ChangeColor(true);
		SetVisibility(true);
		
		if (tileChecker.IsColliding()) {
			Area3D collider = tileChecker.GetCollider() as Area3D;
			
			if (collider.CollisionLayer == 32) {
				SetVisibility(false);
				return;
			}
			
			if (collider.CollisionLayer == 2) {
				ChangeColor(false);
			}
		}
	}
}
