using Godot;
using System;

namespace TowerDefense.Tutorial03_LoadData
{
	
	public partial class GameManager : Node
	{
		public static GameManager instance;
		
		private Label _coinsLabel;
		private Label _livesLabel;
		
		private int _coins = 40;
		private int _lives = 50;
		
		public override void _Ready()
		{
			instance = this;
			
			_coinsLabel = GetNode<Label>("CanvasLayer/UI/Stats/Coins/Label");
			_livesLabel = GetNode<Label>("CanvasLayer/UI/Stats/Lives/Label");
			_UpdateUI();
		}
		
		private void _UpdateUI()
		{
			_coinsLabel.Text = $"{_coins}";
			_livesLabel.Text = $"{_lives}";
		}
		
		public bool CanBuyTower(int cost)
		{
			return _coins >= cost;
		}
		
		public bool BuyTower(int cost)
		{
			if (_coins < cost) return false;
			_coins -= cost;
			_UpdateUI();
			return true;
		}
		
		public void OnShipPassed(ShipManager ship)
		{
			_lives -= ship.HP;
			_UpdateUI();
		}
		
		public void OnShipDied(ShipManager ship)
		{
			_coins += ship.reward;
			_UpdateUI();
		}
	}
	
}
