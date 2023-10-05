using Godot;
using System;

namespace Tutorial10_RandomUnits
{

	public partial class UnitSpawner : Node3D
	{
		
		[Export] private PackedScene _unitPrefab;
		[Export] private PackedScene[] _units;
		[Export] private int _nUnits = 4;

		private const int _MIN_X = -6;
		private const int _MAX_X =  6;
		private const int _MIN_Z = -6;
		private const int _MAX_Z =  6;

		private Godot.Collections.Array<Vector3I> _positions;

		public override void _Ready()
		{
			// prepare positions in grid
			_positions = new Godot.Collections.Array<Vector3I>();
			for (int x = _MIN_X; x <= _MAX_X; x += 4) {
				for (int z = _MIN_Z; z <= _MAX_Z; z += 4) {
					_positions.Add(new Vector3I(x, 0, z));
				}
			}

			GD.Randomize();
			_GenerateUnits();
		}

		private void _GenerateUnits()
		{
			// re-randomise positions
			_positions.Shuffle();

			for (int i = 0; i < _nUnits; i++) {
				// spawn unit
				Node3D u = _unitPrefab.Instantiate<Node3D>();
				u.Name = $"Unit_{i}";
				GetNode("UNITS").AddChild(u);

				// random placement
				u.GlobalPosition = _positions[i];
				((Unit)u).SetTargetPosition(u.GlobalPosition);
				u.RotateY(Mathf.DegToRad(GD.RandRange(0, 360)));

				// random visual
				int unitType = (int)(GD.Randi() % _units.Length);
				u.GetNode("Mesh").AddChild(_units[unitType].Instantiate());
			}
		}

		private void _RegenerateUnits()
		{
			foreach (Node n in GetNode("UNITS").GetChildren())
				n.Free();

			_GenerateUnits();
		}

		private void _MoveUnits()
		{
			// re-randomise positions
			_positions.Shuffle();

			int i = 0;
			foreach (Unit u in GetNode("UNITS").GetChildren())
				u.SetTargetPosition(_positions[i++]);
		}
	}

}
