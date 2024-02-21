using Godot;

namespace Tutorial21_MovingPlatform
{

	public partial class Player : CharacterBody3D
	{
		[Export] private float _speed = 2.0f;
		[Export] private float _jumpVelocity = 4.5f;
		private float _gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

		private AnimationTree _anim;
		[Export] public Vector3 velocity;
		private int _rotation;

		public override void _Ready()
		{
			_anim = GetNode<AnimationTree>("AnimationTree");
			_anim.Active = true;
		}

        public override void _Input(InputEvent @event)
        {
			if (@event.IsActionPressed("ui_up"))
				RotationDegrees = Vector3.Zero;
			else if (@event.IsActionPressed("ui_right"))
				RotationDegrees = Vector3.Up * -90f;
			else if (@event.IsActionPressed("ui_down"))
				RotationDegrees = Vector3.Up * 180f;
			else if (@event.IsActionPressed("ui_left"))
				RotationDegrees = Vector3.Up * 90f;
        }

		public override void _PhysicsProcess(double delta)
		{
			velocity = Velocity;

			// Add the gravity.
			if (!IsOnFloor())
				velocity.Y -= _gravity * (float)delta;

			// Handle Jump.
			if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
				velocity.Y = _jumpVelocity;

			// Get the input direction and handle the movement/deceleration.
			// As good practice, you should replace UI actions with custom gameplay actions.
			Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
			Vector3 direction = new Vector3(inputDir.X, 0, inputDir.Y).Normalized();
			if (direction != Vector3.Zero)
			{
				velocity.X = direction.X * _speed;
				velocity.Z = direction.Z * _speed;
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, _speed);
				velocity.Z = Mathf.MoveToward(Velocity.Z, 0, _speed);
			}

			Velocity = velocity;
			MoveAndSlide();
		}
	}

}
