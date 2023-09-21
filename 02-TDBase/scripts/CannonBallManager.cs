using Godot;
using System;

namespace TowerDefense.Tutorial02_Base
{
	
	public partial class CannonBallManager : CharacterBody2D
	{

		private Vector2 _velocity = Vector2.Zero;
		public int damage;
		
		public void Initialize(Vector2 velocity, int damage, float speed)
		{
			_velocity = velocity * speed * 2000;
			this.damage = damage;
		}

		public override void _Process(double delta)
		{
			Velocity = _velocity;
			MoveAndSlide();
		}
		
		private void _OnScreenExited()
		{
			QueueFree();
		}
	}
	
}
