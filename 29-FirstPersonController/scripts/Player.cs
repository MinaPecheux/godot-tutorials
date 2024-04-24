using Godot;

namespace Tutorial29_FirstPersonController
{

	public partial class Player : CharacterBody3D {
		public const float Speed = 2.0f;
		public const float JumpVelocity = 2.5f;
		public const float CamSensitivity = 0.006f;
		public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

		private Node3D _head;
		private Camera3D _cam;

		private bool _sprinting = false;
		
		public override void _Ready() {
			Input.MouseMode = Input.MouseModeEnum.Captured;
			_head = GetNode<Node3D>("Head");
			_cam = GetNode<Camera3D>("Head/Camera3D");
			_cam.Fov = 90;
		}

		public override void _Input(InputEvent @event) {
			if (@event is InputEventMouseMotion m) {
				_head.RotateY(-m.Relative.X * CamSensitivity);
				_cam.RotateX(-m.Relative.Y * CamSensitivity);

				Vector3 camRot = _cam.Rotation;
				camRot.X = Mathf.Clamp(camRot.X,
					Mathf.DegToRad(-80f), Mathf.DegToRad(80f));
				_cam.Rotation = camRot;
			}

			// exit mouse captured mode with Escape
			if (@event is InputEventKey k && k.Keycode == Key.Escape) {
				Input.MouseMode = Input.MouseModeEnum.Visible;
			}
			
			// check for sprint inputs (on/off)
			if (Input.IsActionJustPressed("sprint")) {
				_sprinting = true;
				Tween t = GetTree().CreateTween();
				t.TweenProperty(_cam, "fov", 90, 0.2f)
					.SetEase(Tween.EaseType.InOut)
					.SetTrans(Tween.TransitionType.Cubic);
			}
			else if (Input.IsActionJustReleased("sprint")) {
				_sprinting = false;
				Tween t = GetTree().CreateTween();
				t.TweenProperty(_cam, "fov", 75, 0.2f)
					.SetEase(Tween.EaseType.InOut)
					.SetTrans(Tween.TransitionType.Cubic);
			}

		}

		public override void _PhysicsProcess(double delta) {
			Vector3 velocity = Velocity;

			if (!IsOnFloor())
				velocity.Y -= gravity * (float)delta;

			if (Input.IsActionJustPressed("jump") && IsOnFloor())
				velocity.Y = JumpVelocity;

			Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backwards");
			Vector3 direction = (_head.GlobalTransform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
			// Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
			float speed =  _sprinting ? Speed * 1.4f : Speed;
			if (direction != Vector3.Zero) {
				velocity.X = direction.X * speed;
				velocity.Z = direction.Z * speed;
			} else {
				velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
				velocity.Z = Mathf.MoveToward(Velocity.Z, 0, speed);
			}

			Velocity = velocity;
			MoveAndSlide();
		}
	}

}
