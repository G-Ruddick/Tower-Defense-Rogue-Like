using Godot;
using System;

public partial class TileClass : Node {
	public bool endTile = false;
	public bool isBridge = false;

	[Export] private Area2D tileArea;
	[Export] private AnimatedSprite2D tileSprite;
}
