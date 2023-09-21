using Godot;
using System;

namespace TowerDefense.Tutorial05_MouseFollow
{
	
	public partial class ItemSpawner : Node
	{
		[Export] private PackedScene _itemPrefab;
		
		private PathFollow2D _spawnLocation;
		
		private RandomNumberGenerator _rng;
		
		public override void _Ready()
		{
			_rng = new RandomNumberGenerator();
			
			_spawnLocation = GetNode<PathFollow2D>("SpawnLocation");
			Timer timer = GetNode<Timer>("Timer");
			timer.Timeout += _OnTimerTimeout;
			timer.Start();
		}

		private void _OnTimerTimeout()
		{
			CharacterBody2D item = (CharacterBody2D)_itemPrefab.Instantiate();
			_spawnLocation.Progress = _rng.Randi();
			
			item.Position = _spawnLocation.Position;
			AddChild(item);
		}

	}

}
