using Godot;
using System;

public class CharacterPatrol : Node
{
	private PathFollow _pathFollow;
	private Timer _waypointTimer;
	
	[Export] private float _waypointCheckDistance = 0.1f;
	[Export] private float _totalLoopTime = 5;
	private float _currentPathTime;
	
	private Vector3[] _waypointPositions;
	private int _currentWaypointIndex;
	private int _nextWaypointIndex;
	
	private bool _moving;
	
	public override void _Ready()
	{
		_pathFollow = GetParent<PathFollow>();
		_waypointTimer = GetNode<Timer>("WaypointTimer");
		
		Curve3D pathCurve = _pathFollow.GetParent<Path>().Curve;
		int nWaypoints = pathCurve.GetPointCount();
		_waypointPositions = new Vector3[nWaypoints - 1];
		for (int i = 0; i < nWaypoints - 1; i++)
		{
			_waypointPositions[i] = pathCurve.GetPointPosition(i);
		}
		_currentWaypointIndex = 0;
		_nextWaypointIndex = 1;
		
		_moving = true;
	}
	
	public override void _Process(float delta)
	{
		if (_moving)
		{
			_currentPathTime += delta;
			_pathFollow.UnitOffset = _currentPathTime / _totalLoopTime;
			
			float d = (_waypointPositions[_nextWaypointIndex] - _pathFollow.Translation).Length();
			if (d < _waypointCheckDistance)
			{
				_currentWaypointIndex = _nextWaypointIndex;
				_nextWaypointIndex = (_currentWaypointIndex + 1) % _waypointPositions.Length;
				_waypointTimer.Start();
				_moving = false;
			}
		}
	}

	private void _OnWaypointTimerDone()
	{
		_moving = true;
	}

}
