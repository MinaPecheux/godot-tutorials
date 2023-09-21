using Godot;
using System;

namespace TowerDefense.Tutorial05_MouseFollow
{

	public partial class GameManager : Node
	{
		public static GameManager instance;
		private const int _maxHealth = 3;
		
		[Export] private Texture2D[] _numeralIcons;
	
		[Export] private NodePath _lifeValueIconPath;
		private TextureRect _lifeValueIcon;
		
		[Export] private NodePath _lifeStarsIcon1Path;
		[Export] private NodePath _lifeStarsIcon2Path;
		private TextureRect _lifeStarsIcon1;
		private TextureRect _lifeStarsIcon2;
		
		private int _lives;
		private int _stars;
		
		public override void _Ready()
		{
			instance = this;
			
			_lifeValueIcon = GetNode<TextureRect>(_lifeValueIconPath);
			_lifeStarsIcon1 = GetNode<TextureRect>(_lifeStarsIcon1Path);
			_lifeStarsIcon2 = GetNode<TextureRect>(_lifeStarsIcon2Path);
			_lives = _maxHealth;
			_stars = 0;
		}
		
		public void OnPlayerHit()
		{
			if (_lives == 0) return;
			_lives--;
			_UpdateLifeIcon();
		}
		
		public void OnPlayerHeal()
		{
			if (_lives == _maxHealth) return;
			_lives++;
			_UpdateLifeIcon();
		}
		
		public void LootStar()
		{
			_stars++;
			int tensDigit = (int)(_stars / 10);
			int unitsDigit = _stars % 10;
			_lifeStarsIcon1.Texture = _numeralIcons[tensDigit];
			_lifeStarsIcon2.Texture = _numeralIcons[unitsDigit];
		}
		
		private void _UpdateLifeIcon()
		{
			_lifeValueIcon.Texture = _numeralIcons[_lives];
		}
		
	}

}
