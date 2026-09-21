using Godot;
using System;

public partial class ButtonTemplate : Control {
	[ExportCategory("Animations")]
	[Export] private string mouseInsideAnimation;
	[Export] private string inactiveAnimation;
	[Export] private string defaultAnimation;

	[ExportCategory("Button Components")]
	[Export] private AnimatedSprite2D buttonSprite;
	[Export] private Area2D buttonArea;
	[Export] private CollisionShape2D areaShape;
	[Export] public Label textBox;

	private bool clickable;

	public override void _Ready() {
		// checking for empty components
		if (areaShape.Shape == null) {
			GD.PrintErr(this.Name + "'s CollisionShape2D has no assigned shape.");
			GetTree().Quit();
		}
		if (buttonSprite.SpriteFrames == null) {
			GD.PrintErr(this.Name + "'s AnimatedSprite2D has no assigned sprites.");
			GetTree().Quit();
		}

		// checking for invalid animation names
		if (!buttonSprite.SpriteFrames.HasAnimation(mouseInsideAnimation)) {
			GD.PrintErr(this.Name + " has no animation called " + mouseInsideAnimation + " in the AnimatedSpite2D.");
			GetTree().Quit();
		}
		if (!buttonSprite.SpriteFrames.HasAnimation(inactiveAnimation)) {
			GD.PrintErr(this.Name + " has no animation called " + inactiveAnimation + " in the AnimatedSpite2D.");
			GetTree().Quit();
		}
		if (!buttonSprite.SpriteFrames.HasAnimation(defaultAnimation)) {
			GD.PrintErr(this.Name + " has no animation called " + defaultAnimation + " in the AnimatedSpite2D.");
			GetTree().Quit();
		}

		buttonArea.Visible = true;
		clickable = false;

		buttonArea.MouseEntered += OnMouseEnter;
		buttonArea.MouseExited += OnMouseExit;

		OnMouseExit();
	}

	public bool GetClickability() {
		return (buttonArea.Visible & clickable);
	}

	public void ChangeClickability() {
		buttonArea.Visible = !buttonArea.Visible;
	}

	// changing button sprite to when mouse if hovering over
	private void OnMouseEnter() {
		clickable = true;

		if (buttonArea.Visible) {
			buttonSprite.Play(mouseInsideAnimation);
		}
		else {
			buttonSprite.Play(inactiveAnimation);
		}
	}

	// changing button sprite to when mouse is not over
	private void OnMouseExit() {
		clickable = false;
		if (buttonArea.Visible) {
			buttonSprite.Play(defaultAnimation);
		}
		else {
			buttonSprite.Play(inactiveAnimation);
		}
	}
}
