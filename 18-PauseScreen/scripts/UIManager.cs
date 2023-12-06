using Godot;
using System;

public partial class UIManager : Control
{
	private Control _pausePanel;

    public override void _Ready()
    {
        _pausePanel = GetNode<Control>("PausePanel");
		_pausePanel.Hide();
    }
	
	private void _Pause()
	{
		GetTree().Paused = true;
		_pausePanel.Show();
		_pausePanel
			.GetNode<AnimationPlayer>("AnimationPlayer")
			.Play("start_pause");
	}

	private void _Unpause()
	{
		GetTree().Paused = false;
		_pausePanel.Hide();
	}

}
