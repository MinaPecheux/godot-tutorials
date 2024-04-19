using Godot;

namespace Tutorial27_2DFollowParallax
{

    public partial class Fish : Node2D {

        [Export] private Texture2D[] _textures;

        private float _speed;
        private Sprite2D _sprite;
        private Timer _turnTimer;
        private Timer _waitTimer;
        private int _dir;
        private bool _waiting;

        public override void _Ready() {
            _sprite = GetNode<Sprite2D>("Sprite2D");
            _turnTimer = GetNode<Timer>("TurnTimer");
            _waitTimer = GetNode<Timer>("WaitTimer");

            // get random speed + random texture
            _speed = (float) GD.RandRange(1f, 3f);
            int i = GD.RandRange(0, _textures.Length - 1);
            _sprite.Texture = _textures[i];

            _turnTimer.WaitTime = GD.RandRange(1f, 3f);
            _turnTimer.Start();
            _dir = 1;
            _waiting = false;
        }

        public override void _Process(double delta) {
            if (!_waiting)
                Translate(_dir * Vector2.Right * (float)delta * 100);
        }

        private async void _Turn() {
            bool flipped = !_sprite.FlipH;
            _waiting = true;
            _waitTimer.Start();
            await ToSignal(_waitTimer, Timer.SignalName.Timeout);
            _sprite.FlipH = flipped;
            _dir = -_dir;
            _waiting = false;
        }
    }

}
