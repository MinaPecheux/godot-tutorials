using Godot;
using System;
using System.Collections.Generic;

namespace TowerDefense.Tutorial02_Base
{
	
	public partial class TowerManager : Area2D
	{
		[Export] private PackedScene _cannonBallAsset;
		
		private Node2D _cannonSprite;
		private CollisionShape2D _fovAreaShape;

		private LevelManager _levelManager;
		
		private List<Node2D> _targetsInRange;
		private Node2D _currentTarget;
		
		// tower parameters
		private float _attackRate = 1f;
		private int _attackDamage = 1;
		private float _attackSpeed = 0.25f;
		private float _attackDelay;
		
		public override void _Ready()
		{
			_cannonSprite = GetNode<Node2D>("Cannon");
			
			_targetsInRange = new List<Node2D>();
			_currentTarget = null;
			
			_attackDelay = _attackRate;
		}
		
		public override void _Process(double delta)
		{
			if (_currentTarget != null)
			{
				_cannonSprite.LookAt(_currentTarget.Position);
				
				if (_attackDelay > 0)
				{
					_attackDelay -= (float)delta;
				}
				else
				{
					_FireCannonBall();
					_attackDelay = _attackRate;
				}
			}
		}

		public void Initialize(LevelManager levelManager, float radius)
		{
			_levelManager = levelManager;
			
			_fovAreaShape = GetNode<CollisionShape2D>("FOVArea2D/CollisionShape2D");
			((CircleShape2D)_fovAreaShape.Shape).Radius = radius;
		}
		
		private void _OnTowerMouseEntered()
		{
			_levelManager.SetCanPlaceTower(false);
		}
		
		private void _OnTowerMouseExited()
		{
			_levelManager.SetCanPlaceTower(true);
		}
		
		private void _OnFOVAreaEntered(Area2D area)
		{
			Node2D ship = (Node2D) ((Node) area).GetParent();
			_targetsInRange.Add(ship);
			if (_currentTarget == null)
				_currentTarget = _targetsInRange[0];
		}
		
		private void _OnFOVAreaExited(Area2D area)
		{
			Node2D ship = (Node2D) ((Node) area).GetParent();
			_targetsInRange.Remove(ship);
			
			if (_currentTarget == ship)
			{
				if (_targetsInRange.Count > 0)
					_currentTarget = _targetsInRange[0];
				else
					_currentTarget = null;
			}
		}
		
		private void _FireCannonBall()
		{
			CannonBallManager cannonBall = (CannonBallManager) _cannonBallAsset.Instantiate();
			cannonBall.Position = Position;
			
			Vector2 offsettedTarget = _currentTarget.Position + _currentTarget.Transform.X * 50f;
			Vector2 velocity = (offsettedTarget - Position).Normalized();
			cannonBall.Initialize(velocity, _attackDamage, _attackSpeed);
			
			GetTree().Root.AddChild(cannonBall);
		}
	}
	
}
