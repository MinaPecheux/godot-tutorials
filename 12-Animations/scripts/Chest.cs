using Godot;
using System;

public partial class Chest : Node3D
{
	private Control _rewardPanel;

    public override void _Ready()
    {
		_rewardPanel = GetNode<Control>("RewardPanel");
		_rewardPanel.Visible = false;
    }

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("interact")) {
			_Open();
		}
	}

	private void _Open()
	{
		GetNode<AnimationPlayer>("AnimationPlayer").Play("chest-open");
		GetNode("Label3D").QueueFree();
	}

	private void _ShowRewardPanel()
	{
		_rewardPanel.Visible = true;
	}

	private void _CloseRewardPanel()
	{
		_rewardPanel.Visible = false;
	}
}
