using Godot;
using System;

public partial class Player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	public const float RotationVelocity = 3.5f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	private AnimationTree _anim;
	private AnimationNodeStateMachinePlayback _animPlayback;
	[Export] public Vector3 velocity;

    public override void _Ready()
    {
        _anim = GetNode<AnimationTree>("AnimationTree");
		_animPlayback = (AnimationNodeStateMachinePlayback) _anim.Get("parameters/playback");
		_anim.Active = true;
    }

	public override void _PhysicsProcess(double delta)
	{
		velocity = Velocity;
		bool punched = false;
		
		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;
		else {
			// Handle Jump.
			if (Input.IsActionJustPressed("jump"))
				velocity.Y = JumpVelocity;

			// Handle Punch.
			if (Input.IsActionJustPressed("punch"))
				punched = true;
			_anim.Set("parameters/conditions/punch", punched);
		}

		// Punch? Interrupt movement.
		if (_animPlayback.GetCurrentNode() == "punch") {
			velocity = Vector3.Zero;
			Velocity = velocity;
			return;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		float turnStrength = Input.GetAxis("left", "right");
		float moveStrength = Input.GetAxis("forward", "backwards");

		RotateY(-Mathf.DegToRad(turnStrength * RotationVelocity));

		Vector3 direction = (Transform.Basis * new Vector3(0, 0, moveStrength)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
