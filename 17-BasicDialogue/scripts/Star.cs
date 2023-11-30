using Godot;
using System;

public partial class Star : Node3D
{

	private AnimationPlayer _anim;

	public override void _Ready()
	{
		GetNode<OmniLight3D>("Light").LightEnergy = 0;

		_anim = GetNode<AnimationPlayer>("AnimationPlayer");
		Hide();
	}

	public async void Appear()
	{
		Show();
		_anim.Play("spawn");

		await ToSignal(_anim, AnimationPlayer.SignalName.AnimationFinished);
		_anim.Play("idle");
	}

}
