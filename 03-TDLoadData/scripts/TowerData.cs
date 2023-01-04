using Godot;
using System;

using MonoCustomResourceRegistry;

namespace TowerDefense.Tutorial03_LoadData
{
	
	[RegisteredType(nameof(TowerData), "", nameof(Resource))]
	public class TowerData : Resource
	{
		[Export] public float attackRate { get; set; }
		[Export] public int attackDamage { get; set; }
		[Export] public float attackSpeed { get; set; }
		[Export] public float radius { get; set; }
		[Export] public int cost { get; set; }
		[Export] public Texture sprite { get; set; }
		
		public TowerData()
		{
			attackRate = 1f;
			attackDamage = 1;
			attackSpeed = 0.25f;
			radius = 400f;
			cost = 10;
			sprite = null;
		}
	}
	
}
