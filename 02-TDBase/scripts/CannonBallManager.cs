using Godot;
using System;

public class CannonBallManager : KinematicBody2D
{

	private Vector2 _velocity = Vector2.Zero;
	public int damage;
	
	public void Initialize(Vector2 velocity, int damage, float speed)
	{
		_velocity = velocity * speed * 2000;
		this.damage = damage;
	}

	public override void _Process(float delta)
	{
		MoveAndSlide(_velocity);
	}
	
	private void _OnScreenExited()
	{
		QueueFree();
	}
}
