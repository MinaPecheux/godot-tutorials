using Godot;
using System;

public partial class PlayerShip : CharacterBody2D
{
	private Vector2 _targetPosition;
	private Color _drawColor;
	
	public override void _Ready()
	{
		_targetPosition = Position;
		_drawColor = new Color(1, 1, 1, 0.75f);
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion eventMouseMove)
		{
			_targetPosition = eventMouseMove.Position;
		}
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 move = _targetPosition - Position;
		if (move.Length() > 10f) {
			Velocity = move.Normalized() * 500f;
			MoveAndSlide();
			QueueRedraw(); // refresh the draw visuals
		}
	}
	
	public override void _Draw()
	{
		DrawLine(Vector2.Zero, _targetPosition - Position, _drawColor, 2, true);
		DrawCircle(_targetPosition - Position, 6, _drawColor);
	}
	
}
