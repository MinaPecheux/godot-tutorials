using Godot;

namespace Tutorial25_3DNav
{

	public partial class Player : CharacterBody3D
	{
		[Export] private float _speed = 2.0f;
		[Export] private float _jumpVelocity = 3.5f;
		[Export] private float _rotationVelocity = 3.5f;
		private float _gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

		private AnimationTree _anim;
		private NavigationAgent3D _agent;
		[Export] public Vector3 velocity;
		private int _rotation;

		public override void _Ready()
		{
			_anim = GetNode<AnimationTree>("AnimationTree");
			_agent = GetNode<NavigationAgent3D>("NavigationAgent3D");
			_anim.Active = true;
		}

		public override void _PhysicsProcess(double delta)
		{
			velocity = Velocity;

			if (!IsOnFloor())
				velocity.Y -= _gravity * (float)delta;

			if (!_agent.IsNavigationFinished()) {
				Vector3 next = _agent.GetNextPathPosition();
				LookAt(new Vector3(next.X, GlobalPosition.Y, next.Z), Vector3.Up);
				Vector3 diff = next - GlobalPosition;
				Vector3 dir = diff.Normalized();
				velocity.X = dir.X * _speed;
				velocity.Z = dir.Z * _speed;
			} else {
				velocity.X = 0f;
				velocity.Z = 0f;
			}
			
			Velocity = velocity;
			MoveAndSlide();
		}

		public void SetTargetPosition(Vector3 pos)
		{
			var map = GetWorld3D().NavigationMap;
			var p = NavigationServer3D.MapGetClosestPoint(map, pos);
			_agent.TargetPosition = p;
		}

		public void Jump(Node3D body)
		{
			if (body == this) {
				Vector3 v = Velocity;
				v.Y += _jumpVelocity;
				Velocity = v;
			}
		}
	}

}
