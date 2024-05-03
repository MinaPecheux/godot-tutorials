using Godot;

namespace Tutorial32_Random2DObstacles
{

	public partial class AsteroidSpawner : PathFollow2D {
		[Export] private PackedScene _asteroidPrefab;
		[Export] private Timer _asteroidSpawnTimer;
		private Node _scene;

		public override void _Ready() {
			_scene = GetNode("/root/Root");
		}
		
		private void _SpawnAsteroid() {
			Asteroid asteroid = _asteroidPrefab.Instantiate<Asteroid>();
			_scene.AddChild(asteroid);
			ProgressRatio = (float) GD.RandRange(0.1f, 0.9f);
			asteroid.GlobalPosition = Position;
			asteroid.Initialize();

			_asteroidSpawnTimer.WaitTime = GD.RandRange(0.5f, 2f);
			_asteroidSpawnTimer.Start();
		}
	}

}
