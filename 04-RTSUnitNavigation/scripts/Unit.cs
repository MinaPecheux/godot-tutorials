using Godot;
using System;

public class Unit : KinematicBody
{
	
	private NavigationAgent _agent;
	
	public override void _Ready()
	{
		_agent = GetNode<NavigationAgent>("NavigationAgent");
		_agent.SetTargetLocation(GlobalTranslation);
	}
	
	public void MoveTo(Vector3 pos)
	{
		_agent.SetTargetLocation(pos);
	}
	
	public override void _PhysicsProcess(float delta)
	{
		if (_agent.IsNavigationFinished()) return;
		
		Vector3 currentPos = GlobalTranslation;
		Vector3 nextPos = _agent.GetNextLocation();
		Vector3 velocity = (nextPos - currentPos).Normalized() * _agent.MaxSpeed;
		MoveAndSlide(velocity);
		
		if ((nextPos - currentPos).LengthSquared() > 0.01f) {
			LookAt(nextPos, Vector3.Up);
		}
	}
	
}
