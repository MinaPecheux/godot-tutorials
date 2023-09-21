using Godot;
using System;

public partial class GameManager : Node3D
{
	private const int _GROUND_MASK = 2;
	private Camera3D _camera;
	private UnitTargetLocation _unitTargetLocation;
	private Unit _unit;
	
	public override void _Ready()
	{
		_camera = GetNode<Camera3D>("Camera3D");
		_unitTargetLocation = GetNode<UnitTargetLocation>("UnitTargetLocation");
		_unit = GetNode<Unit>("Units/Unit");
	}
	
	public override void _Input(InputEvent @event)
	{
		if (
			@event is InputEventMouseButton eventMouseButton &&
			eventMouseButton.Pressed &&
			eventMouseButton.ButtonIndex == MouseButton.Right
		) {
			Vector3 origin = _camera.ProjectRayOrigin(eventMouseButton.Position);
			Vector3 direction = _camera.ProjectRayNormal(eventMouseButton.Position);
			Vector3 end = origin + direction * 1000;
			
			PhysicsDirectSpaceState3D state = GetWorld3D().DirectSpaceState;
			PhysicsRayQueryParameters3D queryParams = new PhysicsRayQueryParameters3D();
			queryParams.From = origin; queryParams.To = end;
			queryParams.CollisionMask = _GROUND_MASK;
			var intersection = state.IntersectRay(queryParams);
			if (intersection.Count > 0) {
				// we hit the ground
				Vector3 pos = (Vector3)intersection["position"];
				_unitTargetLocation.Click(pos);
				_unit.MoveTo(pos);
			}
		}
	}
	
}
