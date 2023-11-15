using Godot;
using System;
using System.Collections.Generic;

public partial class WaveManager : Node2D
{

    struct EnemyData
    {
        public Texture2D sprite;
        public int healthpoints;
        public float speed;
    }

    private List<EnemyData> _enemyTypes = new() {
        new EnemyData{ sprite = GD.Load<Texture2D>("res://art/Tanks/tank-white.png"), healthpoints = 1, speed = 1 },
        new EnemyData{ sprite = GD.Load<Texture2D>("res://art/Tanks/tank-blue.png"), healthpoints = 2, speed = 2 },
        new EnemyData{ sprite = GD.Load<Texture2D>("res://art/Tanks/tank-red.png"), healthpoints = 3, speed = 3 },
    };

    private List<(int, int)[]> _waves = new() {
        new (int, int)[] { (0, 5), (1, 3), (2, 2) },
        new (int, int)[] { (0, 6), (1, 4), (2, 3) },
        new (int, int)[] { (0, 3), (1, 2), (0, 2), (2, 1), (1, 2), (2, 4) },
        new (int, int)[] { (0, 2), (1, 3), (0, 1), (2, 1), (1, 3), (0, 3), (1, 2), (2, 2) },
    };

    [Export] private PackedScene _enemyScene;
    [Export] private Timer _enemySpawnTimer;
    private Node _enemiesParent;

    private List<int> _waveData = new();
    private int _currentWavePosition;
    private int _currentWaveIndex;
    private int _waveCount;

    public override void _Ready()
    {
        _enemiesParent = GetNode("Enemies");
        _StartWave(0);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey e && e.Keycode == Key.Space && e.Pressed)
            _StartWave(_currentWaveIndex + 1);
    }

    private void _StartWave(int waveIndex)
    {
        _currentWaveIndex = waveIndex;

        _waveData.Clear();
        foreach((int type, int count) in _waves[waveIndex]) {
            for (int i = 0; i < count; i++) {
                _waveData.Add(type);
            }
        }
        _waveCount = _waveData.Count;

        _currentWavePosition = 0;

        _enemySpawnTimer.Start();
    }

    private void _OnSpawnEnemy()
    {
        if (_currentWavePosition >= _waveData.Count) return;

        int enemyType = _waveData[_currentWavePosition];
        _currentWavePosition++;

        Enemy enemy = _enemyScene.Instantiate<Enemy>();
        _enemiesParent.AddChild(enemy);
        EnemyData d = _enemyTypes[enemyType];
        enemy.Initialize(d.sprite, d.healthpoints, d.speed);
    }

    public void EndEnemy(Node enemy)
    {
        enemy.QueueFree();
        _waveCount--;

        if (_waveCount == 0) _enemySpawnTimer.Stop();
    }
}
