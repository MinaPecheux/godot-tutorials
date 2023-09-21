using Godot;
using System;

namespace TowerDefense.Tutorial05_MouseFollow
{
	
	public partial class CameraShake : Camera2D
	{
		private float _amplitude = 10f;
		private float _shakeTime = 0f;
		
		private RandomNumberGenerator _rng;
		
		public override void _Ready() {
			_rng = new RandomNumberGenerator();
		}
		
		public override void _Process(double delta)
		{
			if (_shakeTime == 0) return;
			
			float x = (float)(_amplitude * _rng.RandfRange(0f, 1f) * (_rng.Randf() < 0.5f ? 1 : -1));
			float y = (float)(_amplitude * _rng.RandfRange(0f, 1f) * (_rng.Randf() < 0.5f ? 1 : -1));
			Offset = new Vector2(x, y);
			
			_shakeTime -= (float)delta;
			if (_shakeTime <= 0) {
				_shakeTime = 0;
				Offset = Vector2.Zero;
			}
		}
		
		public void Shake(float duration) {
			_shakeTime = duration;
		}
	}

}
