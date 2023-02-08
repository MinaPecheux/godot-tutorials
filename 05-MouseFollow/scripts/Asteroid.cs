using Godot;
using System;

namespace TowerDefense.Tutorial05_MouseFollow
{
	
	public class Asteroid : Item
	{
		private CameraShake _camShake;
		
		[Export] private Texture[] _sprites;
		
		protected override void _OnReady() {
			_camShake = GetNode<CameraShake>("/root/Root/Camera2D");
			
			GetNode<Sprite>("Sprite").Texture = _sprites[GD.Randi() % _sprites.Length];
			SetRotation((float)GD.RandRange(0f, 2f * Mathf.Pi));
			SetScale(Vector2.One * (float)GD.RandRange(0.75f, 1.1f));
		}
		
		protected override void _OnCollision()
		{
			_camShake.Shake(0.35f);
			GameManager.instance.OnPlayerHit();
		}

	}

}
