using Godot;

public partial class Enemy : PathFollow2D
{
    private int _healthpoints;
    private float _speed;

    private WaveManager _waveManager;

    public override void _Ready()
    {
        _waveManager = GetNode<WaveManager>("/root/Root");
    }

    public override void _Process(double delta)
    {
        ProgressRatio += _speed * (float) delta * 0.05f;
        if (ProgressRatio == 1) // reached the end of the path
            _waveManager.EndEnemy(this);
    }

    public void Initialize(Texture2D sprite, int healthpoints, float speed)
    {
        GetNode<Sprite2D>("Sprite2D").Texture = sprite;
        _healthpoints = healthpoints;
        _speed = speed;
    }

    private void _OnClicked(Node viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventMouseButton e && e.ButtonIndex == MouseButton.Left && e.Pressed) {
            _healthpoints--;
            if (_healthpoints == 0)
                _waveManager.EndEnemy(this);
        }
    }
}
