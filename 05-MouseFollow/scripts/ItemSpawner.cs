using Godot;
using System;

namespace TowerDefense.Tutorial05_MouseFollow
{
	
	public class ItemSpawner : Node
	{
		[Export] private PackedScene _itemPrefab;
		
		private PathFollow2D _spawnLocation;
		
		public override void _Ready()
		{
			_spawnLocation = GetNode<PathFollow2D>("SpawnLocation");
			Timer timer = GetNode<Timer>("Timer");
			timer.Connect("timeout", this, "_OnTimerTimeout");
			timer.Start();
		}

		private void _OnTimerTimeout()
		{
			KinematicBody2D item = (KinematicBody2D)_itemPrefab.Instance();
			_spawnLocation.Offset = GD.Randi();
			
			item.Position = _spawnLocation.Position;
			AddChild(item);
		}

	}

}
