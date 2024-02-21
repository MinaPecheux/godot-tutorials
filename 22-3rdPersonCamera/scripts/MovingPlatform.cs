using Godot;
using System.Collections.Generic;

namespace Tutorial22_3rdPersonCamera
{

	public partial class MovingPlatform : PathFollow3D
	{
		[Export] private float _speed;
		[Export] private float _pauseTimeAtWaypoints;
		[Export] private bool _jumpToStart;

		private float _totalCurveLength;
		private int _nWaypoints;
		private int _curWaypointIdx;
		private List<float> _waypointRatios;

		private bool _moving = false;
		private float _moveFrom, _moveTo, _moveTime, _moveDelay;
		private int _moveDirection = 1; // 1: forward, -1: backward

		public async override void _Ready()
		{
			Curve3D c = GetParent<Path3D>().Curve;
			_nWaypoints = c.PointCount;
			_totalCurveLength = c.GetBakedLength();
			_waypointRatios = new();
			float curLength = 0;
			for (int i = 0; i < _nWaypoints; i++) {
				if (i > 0) {
					float r1 = c.GetClosestOffset(c.GetPointPosition(i - 1));
					float r2 = c.GetClosestOffset(c.GetPointPosition(i));
					curLength += r2 - r1;
				}
				_waypointRatios.Add(curLength / _totalCurveLength);
			}

			ProgressRatio = 0;
			_curWaypointIdx = 0;

			// wait if need be
			if (_pauseTimeAtWaypoints > 0)
				await ToSignal(GetTree().CreateTimer(_pauseTimeAtWaypoints),
					Timer.SignalName.Timeout);

			_StartMove();
		}

		public override void _Process(double delta)
		{
			if (!_moving) return;

			if (_moveDelay < _moveTime) {
				_moveDelay += (float) delta;
				
				float t = _pauseTimeAtWaypoints > 0
					? _EaseInOutQuart(_moveDelay / _moveTime)
					: _moveDelay / _moveTime;
				ProgressRatio = Mathf.Lerp(_moveFrom, _moveTo, t);
			} else {
				ProgressRatio = _moveTo;
				_moving = false;
				_ReachWaypoint();
			}
		}

		private async void _ReachWaypoint()
		{
			// wait if need be
			if (_pauseTimeAtWaypoints > 0)
				await ToSignal(GetTree().CreateTimer(_pauseTimeAtWaypoints),
					Timer.SignalName.Timeout);
			// restart movement
			_StartMove();
		}

		private void _StartMove()
		{
			_curWaypointIdx += _moveDirection;
			if (_curWaypointIdx == _nWaypoints) {
				if (_jumpToStart)
					_curWaypointIdx = 1;
				else {
					_moveDirection = -1;
					_curWaypointIdx -= 2;
				}
			} else if (_curWaypointIdx == -1) {
				_moveDirection = 1;
				_curWaypointIdx += 2;
			}

			_moveFrom = _waypointRatios[_curWaypointIdx - _moveDirection];
			_moveTo = _waypointRatios[_curWaypointIdx];
			_moveTime = Mathf.Abs(_moveTo - _moveFrom) * _totalCurveLength / _speed;
			_moveDelay = 0;
			_moving = true;
		}

		private float _EaseInOutQuart(float x) {
			return x < 0.5f ? 8 * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 4) / 2;
		}
	}

}
