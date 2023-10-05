using Godot;
using System;

public partial class Game : ColorRect
{
    private Timer _gameOverTimer;
    private Control _endGameUI;
    private ProgressBar _progressBar;

    private float _gameTime;
    private bool _running;

    public override void _Ready()
    {
        GetNode<Button>("CenterContainer/ClickMe").Pressed += () => _EndGame(true);
        _gameOverTimer = GetNode<Timer>("GameOverTimer");
        _gameOverTimer.Timeout += () => _EndGame(false);

        _endGameUI = GetNode<Control>("EndGameUI");
        _endGameUI.Visible = false;

        _endGameUI.GetNode<Button>("MarginContainer/VBoxContainer/HBoxContainer/Back").Pressed +=
            () => GetNode<SceneLoader>("/root/SceneLoader").ChangeToScene("main_menu.tscn");

        _progressBar = GetNode<ProgressBar>("ProgressBar");
        _progressBar.MaxValue = _gameOverTimer.WaitTime;
        _progressBar.Value = 0;
        _gameTime = 0;
        _running = true;
    }

    public override void _Process(double delta) {
        // update progress bar
        if (_running) {
            _gameTime += (float) delta;
            _progressBar.Value = _gameTime;
        }
    }

    private void _EndGame(bool win) {
        // win/game over logic

        _gameOverTimer.Stop();
        _endGameUI.Visible = true;
        _endGameUI.GetNode<Label>("MarginContainer/VBoxContainer/Label").Text =
            win ? "Victory!" : "Game over :(";

        _running = false;
    }

    private void _Replay() {
        _gameOverTimer.Start();
        _endGameUI.Visible = false;

        _gameTime = 0;
        _running = true;
    }
}
