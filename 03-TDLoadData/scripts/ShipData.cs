using Godot;
using System;

using MonoCustomResourceRegistry;

namespace TowerDefense.Tutorial03_LoadData
{
	[RegisteredType(nameof(ShipData), "", nameof(Resource))]
	public partial class ShipData : Resource
	{
		[Export] public float speed { get; set; }
		[Export] public int HP { get; set; }
		[Export] public int reward { get; set; }
		[Export] public Texture2D sprite { get; set; }
		
		public ShipData()
		{
			speed = 1f;
			HP = 1;
			reward = 1;
			sprite = null;
		}
	}
	
}
