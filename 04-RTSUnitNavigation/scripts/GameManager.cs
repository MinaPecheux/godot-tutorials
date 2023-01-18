
using Godot;
using System;

public class GameManager : Spatial
{
	private const int _GROUND_MASK = 2;
	private Camera _camera;
	private UnitTargetLocation _unitTargetLocation;
	private Unit _unit;
	
	public override void _Ready()
	{
		_camera = GetNode<Camera>("Camera");
		_unitTargetLocation = GetNode<UnitTargetLocation>("UnitTargetLocation");
		_unit = GetNode<Unit>("Units/Unit");
	}
	
	public override void _Input(InputEvent @event)
	{
		if (
			@event is InputEventMouseButton eventMouseButton &&
			eventMouseButton.Pressed &&
			(ButtonList)eventMouseButton.ButtonIndex == ButtonList.Right
		) {
			Vector3 origin = _camera.ProjectRayOrigin(eventMouseButton.Position);
			Vector3 direction = _camera.ProjectRayNormal(eventMouseButton.Position);
			Vector3 end = origin + direction * 1000;
			
			PhysicsDirectSpaceState state = GetWorld().DirectSpaceState;
			var intersection = state.IntersectRay(
				origin, end,
				new Godot.Collections.Array {},
				_GROUND_MASK);
			if (intersection.Count > 0) {
				// we hit the ground
				Vector3 pos = (Vector3)intersection["position"];
				_unitTargetLocation.Click(pos);
				_unit.MoveTo(pos);
			}
		}
	}
	
}
