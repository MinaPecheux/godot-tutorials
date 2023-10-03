using Godot;
using System;

public partial class MainMenu : ColorRect
{
	public override void _Ready()
	{
		GetNode<Button>("CenterContainer/VBoxContainer/Play").Pressed +=
			() => GetNode<SceneLoader>("/root/SceneLoader").ChangeToScene("game.tscn");
	}

	public void ExitGame() {
		GetTree().Quit();
	}
}
