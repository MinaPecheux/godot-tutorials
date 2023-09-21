using Godot;
using System;

namespace TowerDefense.Tutorial05_MouseFollow
{
	
	public partial class Asteroid : Item
	{
		private CameraShake _camShake;
		
		[Export] private Texture2D[] _sprites;
		
		private RandomNumberGenerator _rng;
		
		protected override void _OnReady() {
			_rng = new RandomNumberGenerator();
			
			_camShake = GetTree().CurrentScene.GetNode<CameraShake>("Camera2D");
			
			GetNode<Sprite2D>("Sprite2D").Texture = _sprites[GD.Randi() % _sprites.Length];
			Rotation = (float)_rng.RandfRange(0f, 2f * Mathf.Pi);
			Scale = Vector2.One * (float)_rng.RandfRange(0.75f, 1.1f);
		}
		
		protected override void _OnCollision()
		{
			_camShake.Shake(0.35f);
			GameManager.instance.OnPlayerHit();
		}

	}

}
