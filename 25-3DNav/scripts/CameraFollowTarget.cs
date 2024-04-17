using Godot;

public partial class CameraFollowTarget : Camera3D
{
	[Export] private Node3D _target;
	private Vector3 _offset;

	public override void _Ready()
	{
		if (_target == null) {
			GD.PrintErr("No target for the camera!");
			return;
		}

		_offset = GlobalPosition - _target.GlobalPosition;
	}

	public override void _Process(double delta)
	{
		GlobalPosition = _target.GlobalPosition + _offset;
	}
}
