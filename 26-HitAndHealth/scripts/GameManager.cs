using Godot;

namespace Tutorial26_HitAndHealth
{

    public partial class GameManager : Control
    {
        [Export] private PackedScene _scoreLabelPrefab;
        [Export] private ProgressBar _healthbar;

        private TextureRect _buffaloIcon;
        private AnimationPlayer _buffaloAnim;

        private bool _buffaloIsAlive = true;

        public override void _Ready()
        {
            _buffaloIcon = GetNode<TextureRect>("Buffalo");
            _buffaloAnim = GetNode<AnimationPlayer>("Buffalo/AnimationPlayer");
            _healthbar.Value = _healthbar.MaxValue;
        }

        private void _OnBuffaloGuiInput(InputEvent @event)
        {
            if (!_buffaloIsAlive) return;
            if (@event is InputEventMouseButton e && e.Pressed
                && e.ButtonIndex == MouseButton.Left) {
                // buffalo icon was clicked!
                _buffaloAnim.Play("take-hit");

                int dmg = GD.RandRange(8, 12);
                _SpawnScoreLabel(dmg, e.GlobalPosition);
                _healthbar.Value -= dmg;
                if (_healthbar.Value <= 0) {
                    _buffaloIsAlive = false;
                    _buffaloIcon.Texture = GD.Load<Texture2D>(
                        "res://26-HitAndHealth/art/sprites/buffalo_dead.png");
                }
            }
        }

        private async void _SpawnScoreLabel(float value, Vector2 pos)
        {
            Label l = _scoreLabelPrefab.Instantiate<Label>();
            l.Text = $"{value}";
            AddChild(l);
            l.GlobalPosition = pos;

            // animate + destroy with tween
            Tween tween = GetTree().CreateTween();
            tween.SetParallel(true);
            tween.TweenProperty(l, "position:y", pos.Y + 300, 0.5f)
                .SetEase(Tween.EaseType.In)
                .SetTrans(Tween.TransitionType.Back);
            tween.TweenProperty(l, "position:x", pos.X + GD.RandRange(-50, 50), 0.5f);
            tween.TweenProperty(l, "modulate:a", 0, 0.45f);

            await ToSignal(tween, Tween.SignalName.Finished);
            l.QueueFree();
        }

    }

}
