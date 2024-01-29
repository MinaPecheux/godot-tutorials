using Godot;

namespace MiniGame_Mini2DPlatformer
{

	public partial class Slime : Area2D
	{
		[Export] private AnimatedSprite2D _sprite;
		[Export] private Timer _timer;
		[Export] private float _speed = 80.0f;
		
		private Vector2[] _waypoints;
		private int _curWaypointIdx = 0;
		private bool _waiting = false;

		public void Initialize(TileMap t, Vector2I pos)
		{
			// compute two waypoints: left and right of the initial position
			// (while avoiding tilemap walls or holes)
			_waypoints = new Vector2[2];

			int idx = 0;
			foreach (int dir in new int[] { -1, 1 }) {
				int offset = 1;
				Vector2I testedCell = new();
				while (offset <= 3) {
					testedCell = pos + Vector2I.Right * dir * offset;
					if (
						t.GetCellSourceId(0, testedCell) != -1 ||
						t.GetCellSourceId(0, testedCell + Vector2I.Down) == -1
					) break;
					offset++;
				}

				_waypoints[idx] = t.MapToLocal(pos + Vector2I.Right * dir * (offset - 1));
				idx++;
			}
		}

		public override void _Process(double delta)
		{
			if (_waiting) return;

			Vector2 w = _waypoints[_curWaypointIdx];
			if (Mathf.IsZeroApprox((w - Position).Length())) {
				_waiting = true;
				_timer.Start();
				_sprite.Stop();
			} else {
				Position = Position.MoveToward(w, (float) (_speed * delta));
			}
		}

		private void _OnWaitTimeout()
		{
			_curWaypointIdx = 1 - _curWaypointIdx;
			_waiting = false;
			_sprite.Play();
			_sprite.FlipH = !_sprite.FlipH;
		}
	}

}
