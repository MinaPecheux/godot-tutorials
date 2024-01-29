using Godot;

namespace MiniGame_Mini2DPlatformer
{

	public partial class GameManager : Control
	{
		[Export] private PackedScene _coinPrefab;
		[Export] private PackedScene _enemyPrefab;
		[Export] private Texture2D[] _lifeIcons;
		[Export] private TextureRect[] _lifeTextureRects;
		[Export] private Label _coinAmount;
		[Export] private AnimationPlayer _levelCompletedAnimator;
		[Export] private AnimationPlayer _gameOverAnimator;
		[Export] private Label _levelCompletedTimeLabel;

		private float _life = 3;
		private int _coins = 0;

		private Vector2 _startPos;
		private float _startTime;

		public override void _Ready()
		{
			TileMap t = GetNode<TileMap>("TileMap");
			var cells = t.GetUsedCells(1); // get all item tiles
			foreach (Vector2I cell in cells) {
				Vector2I atlasCoords = t.GetCellAtlasCoords(1, cell);
				// spawn coin
				if (atlasCoords.X == 0 && atlasCoords.Y == 8) {
					t.SetCell(1, cell, -1);
					Area2D coin = _coinPrefab.Instantiate<Area2D>();
					AddChild(coin);
					coin.Position = t.MapToLocal(cell);
					coin.BodyEntered += (Node2D body) => _OnCoinBodyEntered(coin, body);
				}
				// spawn enemy
				else if (atlasCoords.X == 0 && atlasCoords.Y == 5) {
					t.SetCell(1, cell, -1);

					Slime enemy = _enemyPrefab.Instantiate<Slime>();
					AddChild(enemy);
					enemy.Position = t.MapToLocal(cell);
					enemy.Initialize(t, cell);
					enemy.BodyEntered += _OnEnemyBodyEntered;
				}
			}

			cells = t.GetUsedCells(2); // get all trigger tiles
			foreach (Vector2I cell in cells) {
				var data = t.GetCellTileData(2, cell);
				int type = data.GetCustomData("type").AsInt32();
				if (type == 1) {
					_startPos = t.MapToLocal(cell);
				} else if (type == 2) {
					Area2D exitPoint = new();
					AddChild(exitPoint);
					CollisionShape2D coll = new();
					exitPoint.AddChild(coll);
					coll.Shape = new CircleShape2D() { Radius = 35 };
					exitPoint.Position = t.MapToLocal(cell) - 17.5f * Vector2.One;
					exitPoint.BodyEntered += _OnExitPointBodyEntered;
				}
			}

			_Setup();
		}

		private void _Setup()
		{
			_coins = 0;
			_life = 3;
			_UpdateUICoins();
			_UpdateUILife();
			GetNode<Node2D>("Player").Position = _startPos;
			_startTime = Time.GetTicksMsec();
		}

	#region Area2D Callbacks
		private void _OnCoinBodyEntered(Node2D coin, Node2D body)
		{
			if (body.IsInGroup("players")) {
				coin.QueueFree();
				_coins++;
				_UpdateUICoins();
			}
		}

		private void _OnExitPointBodyEntered(Node2D body)
		{
			if (body.IsInGroup("players")) {
				_LevelCompleted();
			}
		}

		private void _OnEnemyBodyEntered(Node2D body)
		{
			if (body.IsInGroup("players")) {
				_life -= 0.5f;
				_UpdateUILife();
				if (Mathf.IsZeroApprox(_life))
					_GameOver();
			}
		}
	#endregion

	#region UI Updates
		public void _UpdateUICoins()
		{
			_coinAmount.Text = $"x{_coins}";
		}

		public void _UpdateUILife()
		{
			bool halfLife = Mathf.IsEqualApprox(_life - (int) _life, 0.5f);
			int intLife = (int) _life;
			for (int i = 0; i < 3; i++) {
				Texture2D tex = _lifeIcons[2];
				if (i < intLife) tex = _lifeIcons[0];
				else if (i == intLife && halfLife) tex = _lifeIcons[1];
				_lifeTextureRects[i].Texture = tex;
			}
		}
	#endregion

	#region Win/Game Over/Replay
		private void _LevelCompleted()
		{
			float t = (Time.GetTicksMsec() - _startTime) / 1000f;
			int min = (int)(t / 60f);
			int sec = (int)(t - min * 60f);

			_levelCompletedTimeLabel.Text = $"{min.ToString("00")}:{sec.ToString("00")}";
			_levelCompletedAnimator.Play("show");
			GetTree().Paused = true;
		}

		private void _GameOver()
		{
			_gameOverAnimator.Play("show");
			GetTree().Paused = true;
		}

		private void _Replay()
		{
			_Setup();
			_levelCompletedAnimator.Stop();
			_gameOverAnimator.Stop();
			GetTree().Paused = false;
		}
	#endregion

	}

}
