using Godot;
using System;

namespace TowerDefense.Tutorial05_MouseFollow
{
	
	public partial class Item : CharacterBody2D
	{
		protected Vector2 _velocity;
		
		private RandomNumberGenerator _rng;
		
		public override void _Ready() {
			_rng = new RandomNumberGenerator();
			
			_velocity = new Vector2(0, (float)_rng.RandfRange(90f, 130f));
			_OnReady();
		}
		
		public override void _PhysicsProcess(double delta)
		{
			KinematicCollision2D collision = MoveAndCollide(_velocity * (float)delta);
			if (collision != null) {
				_OnCollision();
				QueueFree();
			}
		}
		
		private void _OnScreenExited() { QueueFree(); }
		protected virtual void _OnReady() {}
		protected virtual void _OnCollision() {}

	}

}
