using Godot;
using System;

public partial class Unit : CharacterBody3D
{
	
	private NavigationAgent3D _agent;
	public Vector3 MovementTarget
	{
		get { return _agent.TargetPosition; }
		set { _agent.TargetPosition = value; }
	}
	
	public async override void _Ready()
	{
		base._Ready();
		
		_agent = GetNode<NavigationAgent3D>("NavigationAgent3D");
		
		// These values need to be adjusted for the actor's speed
		// and the navigation layout.
		_agent.PathDesiredDistance = 0.5f;
		_agent.TargetDesiredDistance = 0.5f;

		// Make sure to not await during _Ready.
		Callable.From(ActorSetup).CallDeferred();
	}
	
	private async void ActorSetup()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		MovementTarget = GlobalPosition;
	}
	
	public void MoveTo(Vector3 pos)
	{
		MovementTarget = pos;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		if (_agent.IsNavigationFinished()) return;

		Vector3 currentAgentPosition = GlobalPosition;
		Vector3 nextPathPosition = _agent.GetNextPathPosition();
		Vector3 newVelocity = (nextPathPosition - currentAgentPosition).Normalized();
		newVelocity *= _agent.MaxSpeed;
		Velocity = newVelocity;
		MoveAndSlide();
		
		if ((nextPathPosition - currentAgentPosition).LengthSquared() > 0.01f) {
			LookAt(nextPathPosition, Vector3.Up);
		}
	}
	
}
