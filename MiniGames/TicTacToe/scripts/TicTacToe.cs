using Godot;
using System;

public partial class TicTacToe : ColorRect
{

	[Export] private TextureRect[] _playerMarkers;
	[Export] private Texture2D[] _playerSymbols;
	[Export] private Control _board;
	[Export] private Control _endGameUI;

	int _player; // 0: cross, 1: circle
	int[,] _grid;
	int _remainingSpaces;
	
	public override void _Ready()
	{
		_SetPlayer(0);

		_grid = new int[3, 3];
		for (int x = 0; x < 3; x++)
			for (int y = 0; y < 3; y++)
				_grid[x, y] = -1; // (no owner)
		_remainingSpaces = 9;

		_endGameUI.Hide();
	}

	private void _SetPlayer(int id)
	{
		_player = id;
		
		// toggle player markers to show active _player
		_playerMarkers[id].Modulate = new Color(1f, 1f, 1f, 1f);
		_playerMarkers[1 - id].Modulate = new Color(1f, 1f, 1f, 0.2f);
	}

	private void _Place(int cell)
	{
		int x = cell % 3, y = (int)(cell / 3);
		_grid[x, y] = _player;
		_remainingSpaces--;

        // if line of 3: it's a win!
		if (_CheckForWin(x, y)) {
			_EndGame(false);
			return;
		}

		// if the board is complete: it's a tie!
		if (_remainingSpaces == 0) {
			_EndGame(true);
			return;
		}

		Button b = _board.GetChild<Button>(cell);
		b.Icon = _playerSymbols[_player];
		b.Disabled = true;

		_SetPlayer(1 - _player);
	}

	private bool _CheckForWin(int x, int y)
	{
		int col = 0, row = 0, ldiag = 0, rdiag = 0;

		for (int i = 0; i < 3; i++) {
			if (_grid[x, i] 	== _player) col++;
			if (_grid[i, y] 	== _player) row++;
			if (_grid[i, i] 	== _player) ldiag++;
			if (_grid[i, 2 - i] == _player) rdiag++;
		}

		return col == 3 || row == 3 || ldiag == 3 || rdiag == 3;
	}

	private void _EndGame(bool tie)
	{
		Label l = _endGameUI.GetNode<Label>("PanelContainer/VBoxContainer/Message");
		TextureRect t = _endGameUI.GetNode<TextureRect>("PanelContainer/VBoxContainer/WinnerMarker");

		if (tie) {
			l.Text = "TIE!";
			t.Hide();
		}
		else {
			l.Text = "VICTORY!";
			t.Texture = _playerSymbols[_player];
			t.Show();
		}

		_endGameUI.Show();
	}

	private void _Replay()
	{
		_player = 0;

		for (int i = 0; i < 9; i++) {
			// reset data
			_grid[i % 3, i / 3] = -1;

			// reset UI
			Button b = _board.GetChild<Button>(i);
			b.Icon = null;
			b.Disabled = false;
		}
		_remainingSpaces = 9;

		_endGameUI.Hide();
	}
}
