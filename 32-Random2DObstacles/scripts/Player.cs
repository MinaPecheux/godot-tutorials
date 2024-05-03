using Godot;

namespace Tutorial32_Random2DObstacles
{

	public partial class Player : CharacterBody2D {
		public const float Speed = 600.0f;

		public override void _PhysicsProcess(double delta) {
			Vector2 velocity = Velocity;

			float direction = Input.GetAxis("ui_left", "ui_right");
			if (!Mathf.IsZeroApprox(direction)) {
				velocity.X = direction * Speed;
			} else {
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			}

			Velocity = velocity;
			MoveAndSlide();
		}

		private void _OnArea2DBodyEntered(Node2D body) {
			if (body.IsInGroup("asteroids")) {
				body.QueueFree();
			}
		}
	}

}
