using Godot;
using System;

namespace TowerDefense.Tutorial03_LoadData
{
	
	public class LevelManager : Node2D
	{
		[Export] private PackedScene _shipAsset;
		[Export] private PackedScene _towerAsset;
		private PackedScene _towerButtonAsset;
		[Export] private ShipData[] _shipsData;
		private TowerData[] _towersData;
		private TowerToPlaceManager _towerToPlace;
		
		private Path2D _path;
		
		private bool _isBuilding;
		private bool _canPlaceTower;
		
		private TileMap _groundTilemap;
		private float _cellRound;
		private Vector2 _cellOffset;
		private bool _towerHasValidPlacement;
		private TowerData _towerToPlaceData;
		
		public override void _Ready()
		{
			_path = GetNode<Path2D>("Path2D");
			_towerToPlace = GetNode<TowerToPlaceManager>("/root/Base/Tower-ToPlace");
			_groundTilemap = GetNode<TileMap>("Tilemaps/Ground");
			
			_cellRound = _groundTilemap.CellSize.x;
			_cellOffset = 0.5f * new Vector2(_cellRound, _cellRound);
			
			SetIsBuilding(false);
			
			// dynamically create UI tower buttons + connect to placing logic
			Node towerButtonsParent = GetNode<Node>("/root/Base/CanvasLayer/UI/Towers");
			foreach (TowerData data in _towersData)
			{
				Control c = (Control) _towerButtonAsset.Instance();
				((Button)c).Icon = data.sprite;
				c.GetNode<Label>("Coins/Label").Text = $"{data.cost}";
				c.Connect("pressed", this, "_OnTowerButtonMousePressed", new Godot.Collections.Array() { data });
				c.Connect("mouse_entered", this, "_OnTowerButtonMouseEntered");
				c.Connect("mouse_exited", this, "_OnTowerButtonMouseExited");
				towerButtonsParent.AddChild(c);
			}
		}
		
		private void _OnEnemySpawn()
		{
			Node ship = _shipAsset.Instance();
			Random random = new Random();
			ShipData data = _shipsData[random.Next(0, _shipsData.Length)];
			((ShipManager)ship).Initialize(data);
			_path.AddChild(ship);
		}
		
		public override void _Input(InputEvent @event)
		{
			if (
				@event is InputEventMouseButton eventMouseButton &&
				eventMouseButton.ButtonIndex == 1 &&
				!eventMouseButton.Pressed
			)
			{
				if (_towerHasValidPlacement && _isBuilding && _canPlaceTower)
				{
					if (GameManager.instance.BuyTower(_towerToPlaceData.cost))
						_PlaceTower(_RoundPositionToTilemap(GetGlobalMousePosition()));
				}
			}
			else if (@event is InputEventMouseMotion eventMouseMove)
			{
				Vector2 mousePos = GetGlobalMousePosition();
				_towerToPlace.Position = _RoundPositionToTilemap(mousePos);
				
				// check tower has valid placement
				Vector2 cellPos = _groundTilemap.WorldToMap(_towerToPlace.Position);
				_towerHasValidPlacement = _groundTilemap.GetCellv(cellPos) != -1;
				_towerToPlace.SetValid(_towerHasValidPlacement);
			}
		}
		
		private Vector2 _RoundPositionToTilemap(Vector2 p)
		{
			return (p / _cellRound).Floor() * _cellRound + _cellOffset;
		}
		
		private void _PlaceTower(Vector2 pos)
		{
			Node2D tower = (Node2D) _towerAsset.Instance();
			tower.Position = pos;
			AddChild(tower);
			((TowerManager)tower).Initialize(this, _towerToPlaceData);
			SetIsBuilding(false);
		}
		
		public void SetIsBuilding(bool isBuilding)
		{
			_isBuilding = isBuilding;
			if (isBuilding && _canPlaceTower)
				((CanvasItem)_towerToPlace).Show();
			else
				((CanvasItem)_towerToPlace).Hide();
		}
		
		public void SetCanPlaceTower(bool canPlaceTower)
		{
			_canPlaceTower = canPlaceTower;
			if (canPlaceTower && _isBuilding)
				((CanvasItem)_towerToPlace).Show();
			else
				((CanvasItem)_towerToPlace).Hide();
		}
		
		private void _OnTowerButtonMousePressed(TowerData data)
		{
			if (GameManager.instance.CanBuyTower(data.cost))
			{
				_towerToPlace.SetTowerData(data);
				_towerToPlaceData = data;
				SetIsBuilding(true);
			}
		}
		
		private void _OnTowerButtonMouseEntered()
		{
			SetCanPlaceTower(false);
		}
		
		private void _OnTowerButtonMouseExited()
		{
			SetCanPlaceTower(true);
		}
	}

	
}
