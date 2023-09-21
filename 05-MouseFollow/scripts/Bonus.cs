using Godot;
using System;

namespace TowerDefense.Tutorial05_MouseFollow
{
	
	public enum BonusType
	{
		Life = 0,
		Star,
		
		_NBonusTypes
	}
	
	public partial class Bonus : Item
	{
		[Export] private BonusType _type;
		[Export] private Texture2D[] _sprites;
		
		protected override void _OnReady() {
			_type = (BonusType)(GD.Randi() % (int)BonusType._NBonusTypes);
			GetNode<Sprite2D>("Sprite2D").Texture = _sprites[(int)_type];
		}
		
		protected override void _OnCollision()
		{
			if (_type == BonusType.Life)
				GameManager.instance.OnPlayerHeal();
			else if (_type == BonusType.Star)
				GameManager.instance.LootStar();
		}
	}

}
