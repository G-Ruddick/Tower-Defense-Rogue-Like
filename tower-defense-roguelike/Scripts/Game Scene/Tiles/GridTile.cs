using Godot;
using System;

public partial class GridTile : Node3D {
	[Export] private Sprite3D tileSpite;

	public void ChangeColor(bool validPlacement) {
		if (validPlacement) {
			tileSpite.Modulate = Colors.White;
			return;
		}

		tileSpite.Modulate = new Color(0.74f, 0, 0, 1);
	}

	public void SetVisibility(bool visibility) {
		this.Visible = visibility;
	}
}
