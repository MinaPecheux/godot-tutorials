using Godot;

namespace Tutorial28_EndlessLevel {

	public partial class Player : CharacterBody3D {
		public const float Speed = 1f;
		public const float JumpVelocity = 4.2f;
		public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

		private AnimationTree _anim;
		private AnimationNodeStateMachinePlayback _animPlayback;

		public override void _Ready() {
			_anim = GetNode<AnimationTree>("AnimationTree");
			_animPlayback = (AnimationNodeStateMachinePlayback) _anim.Get("parameters/playback");
		}

		public override void _PhysicsProcess(double delta) {
			Vector3 velocity = Velocity;

			if (!IsOnFloor())
				velocity.Y -= gravity * (float)delta;
			if (Input.IsActionJustPressed("ui_accept") && IsOnFloor()) {
				velocity.Y = JumpVelocity;
				_animPlayback.Travel("jump");
			}
			
			Velocity = velocity;
			MoveAndSlide();
		}
	}

}
