using Godot;
using System;

public class GameManager : Node
{
	public static GameManager instance;
	
	private const int _TOWER_COST = 20;
	
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
	
	public bool CanBuyTower()
	{
		return _coins >= _TOWER_COST;
	}
	
	public bool BuyTower()
	{
		if (_coins < _TOWER_COST) return false;
		_coins -= _TOWER_COST;
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
