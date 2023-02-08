using Godot;
using System;

namespace TowerDefense.Tutorial05_MouseFollow
{
	
	public class Item : KinematicBody2D
	{
		protected Vector2 _velocity;
		
		public override void _Ready() {
			_velocity = new Vector2(0, (float)GD.RandRange(90f, 130f));
			_OnReady();
		}
		
		public override void _PhysicsProcess(float delta)
		{
			KinematicCollision2D collision = MoveAndCollide(_velocity * delta);
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
