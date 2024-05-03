using Godot;

namespace Tutorial32_Random2DObstacles
{

	public partial class Asteroid : CharacterBody2D {
		[Export] private Texture2D[] _asteroidSprites;
		private float _rotation;

		public void Initialize() {
			Sprite2D sprite = GetNode<Sprite2D>("Sprite2D");
			sprite.Texture = _asteroidSprites[
				GD.RandRange(0, _asteroidSprites.Length - 1)];
			sprite.RotationDegrees = (float) GD.RandRange(0f, 360f);
			Velocity = Vector2.Down * 150f;
			_rotation = (float) GD.RandRange(0.005f, 0.02f) * (GD.Randf() < 0.5f ? 1f : -1f);
		}

		public override void _PhysicsProcess(double delta) {
			Rotate(_rotation);
			MoveAndSlide();
		}

		private void _OnScreenExited() {
			QueueFree();
		}
	}

}
