using Godot;

public partial class ThirdPersonCamera : Node3D
{
	[Export] private Node3D _target = null;
	private Node3D _cam;
	private RayCast3D _ray;

	public override void _Ready()
	{
		if (_target == null)
			_target = GetParent<Node3D>();
		_cam = GetNode<Node3D>("Camera3D");
		_ray = GetNode<RayCast3D>("RayCast3D");
		if (_target is CollisionObject3D o)
			_ray.AddException(o);
	}

	public override void _Process(double delta)
	{
		if (_ray.IsColliding()) {
			_cam.GlobalPosition = _ray.GetCollisionPoint();
		} else {
			_cam.GlobalPosition = GlobalPosition;
		}
	}
}
