using Godot;
using System;

namespace TowerDefense.Tutorial03_LoadData
{
	
	public partial class ShipManager : PathFollow2D
	{
		private PathFollow2D _pathFollow;
		
		// ship parameters
		private float _speed = 1f;
		public int HP = 1;
		public int reward = 1;
		
		public override void _Ready()
		{
			_pathFollow = GetNode<PathFollow2D>(GetPath());
		}
		
		public void Initialize(ShipData data)
		{
			_speed = data.speed;
			HP = data.HP;
			reward = data.reward;
			GetNode<Sprite2D>("Sprite2D").Texture = data.sprite;
		}

		public override void _Process(double delta)
		{
			_pathFollow.ProgressRatio += (float)delta * _speed * 0.03f;
			if (_pathFollow.ProgressRatio >= 1)
				_Pass();
		}
		
		private void _Pass()
		{
			GameManager.instance.OnShipPassed(this);
			QueueFree();
		}
		
		private void _OnArea2DBodyEntered(Node2D body)
		{
			CannonBallManager cannonBall = (CannonBallManager) body;
			_TakeHit(cannonBall.damage);
			cannonBall.QueueFree();
		}
		
		private void _TakeHit(int damage)
		{
			HP -= damage;
			if (HP <= 0)
				_Die();
		}
		
		private void _Die()
		{
			GameManager.instance.OnShipDied(this);
			QueueFree();
		}
	}
	
}
