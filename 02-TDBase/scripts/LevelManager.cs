using Godot;
using System;

namespace TowerDefense.Tutorial02_Base
{
	
	public partial class LevelManager : Node2D
	{
		[Export] private PackedScene _shipAsset;
		[Export] private PackedScene _towerAsset;
		private TowerToPlaceManager _towerToPlace;
		
		private Path2D _path;
		
		private bool _isBuilding;
		private bool _canPlaceTower;
		
		private TileMap _groundTilemap;
		private float _cellRound;
		private Vector2 _cellOffset;
		private bool _towerHasValidPlacement;
		
		public override void _Ready()
		{
			_path = GetNode<Path2D>("Path2D");
			_towerToPlace = GetNode<TowerToPlaceManager>("/root/Base/Tower-ToPlace");
			_groundTilemap = GetNode<TileMap>("Tilemaps/Ground");
			
			_cellRound = _groundTilemap.CellQuadrantSize;
			_cellOffset = 0.5f * new Vector2(_cellRound, _cellRound);
			
			SetIsBuilding(false);
			
			// dynamically connect UI tower buttons to placing logic
			Node towerButtonsParent = GetNode<Node>("/root/Base/CanvasLayer/UI/Towers");
			for (int i = 0; i < towerButtonsParent.GetChildCount(); i++)
			{
				Control c = (Control) towerButtonsParent.GetChild(i);
				c.Connect("pressed", new Callable(this, "_OnTowerButtonMousePressed"));
				c.Connect("mouse_entered", new Callable(this, "_OnTowerButtonMouseEntered"));
				c.Connect("mouse_exited", new Callable(this, "_OnTowerButtonMouseExited"));
			}
		}
		
		private void _OnEnemySpawn()
		{
			Node ship = _shipAsset.Instantiate();
			_path.AddChild(ship);
		}
		
		public override void _Input(InputEvent @event)
		{
			if (
				@event is InputEventMouseButton eventMouseButton &&
				eventMouseButton.ButtonIndex == MouseButton.Left &&
				!eventMouseButton.Pressed
			)
			{
				if (_towerHasValidPlacement && _isBuilding && _canPlaceTower)
				{
					if (GameManager.instance.BuyTower())
						_PlaceTower(_RoundPositionToTilemap(GetGlobalMousePosition()));
				}
			}
			else if (@event is InputEventMouseMotion eventMouseMove)
			{
				Vector2 mousePos = GetGlobalMousePosition();
				_towerToPlace.Position = _RoundPositionToTilemap(mousePos);
				
				// check tower has valid placement
				_towerHasValidPlacement = _groundTilemap.GetCellSourceId(
					0, _groundTilemap.LocalToMap(_towerToPlace.Position)) != -1;
				_towerToPlace.SetValid(_towerHasValidPlacement);
			}
		}
		
		private Vector2 _RoundPositionToTilemap(Vector2 p)
		{
			return (p / _cellRound).Floor() * _cellRound + _cellOffset;
		}
		
		private void _PlaceTower(Vector2 pos)
		{
			Node2D tower = (Node2D) _towerAsset.Instantiate();
			tower.Position = pos;
			AddChild(tower);
			((TowerManager)tower).Initialize(this, _towerToPlace.radius);
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
		
		private void _OnTowerButtonMousePressed()
		{
			if (GameManager.instance.CanBuyTower())
				SetIsBuilding(true);
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
