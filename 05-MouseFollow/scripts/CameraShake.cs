using Godot;
using System;

namespace TowerDefense.Tutorial05_MouseFollow
{
	
	public class CameraShake : Camera2D
	{
		private float _amplitude = 10f;
		private float _shakeTime = 0f;
		
		public override void _Process(float delta)
		{
			if (_shakeTime == 0) return;
			
			float x = (float)(_amplitude * GD.RandRange(0f, 1f) * (GD.Randf() < 0.5f ? 1 : -1));
			float y = (float)(_amplitude * GD.RandRange(0f, 1f) * (GD.Randf() < 0.5f ? 1 : -1));
			SetOffset(new Vector2(x, y));
			
			_shakeTime -= delta;
			if (_shakeTime <= 0) {
				_shakeTime = 0;
				SetOffset(Vector2.Zero);
			}
		}
		
		public void Shake(float duration) {
			_shakeTime = duration;
		}
	}

}
