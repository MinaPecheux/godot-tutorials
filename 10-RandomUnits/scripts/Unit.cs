using Godot;
using System;

namespace Tutorial10_RandomUnits
{

	public partial class Unit : Node3D
	{

		private const float _SPEED = 5f;
		private Vector3 _targetPosition;

		public override void _Process(double delta)
		{
			Vector3 d = _targetPosition - GlobalPosition;
			if (d.Length() > 0.1f) {
				GlobalTranslate(d.Normalized() * _SPEED * (float)delta);
			}
		}

		public void SetTargetPosition(Vector3 p)
		{
			_targetPosition = p;
			Vector3 d = _targetPosition - GlobalPosition;
			if (d.Length() > 0.1f) {
				LookAt(_targetPosition, Vector3.Up);
			}
		}
	}

}
