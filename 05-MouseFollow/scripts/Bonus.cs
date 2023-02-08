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
	
	public class Bonus : Item
	{
		[Export] private BonusType _type;
		[Export] private Texture[] _sprites;
		
		protected override void _OnReady() {
			_type = (BonusType)(GD.Randi() % (int)BonusType._NBonusTypes);
			GetNode<Sprite>("Sprite").Texture = _sprites[(int)_type];
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
