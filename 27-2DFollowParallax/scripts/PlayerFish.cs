using Godot;

namespace Tutorial27_2DFollowParallax
{

    public partial class PlayerFish : Node2D {
        private const float Speed = 3f;
        private Sprite2D _sprite;
        private Vector2 _mapBounds;

        public override void _Ready() {
            _sprite = GetNode<Sprite2D>("Sprite2D");

            // get camera screen limits to know map bounds
            Camera2D cam = GetNode<Camera2D>("Camera2D");
            float spriteHalfWidth = _sprite.Scale.X * _sprite.GetRect().Size.X * 0.5f;
            _mapBounds = new Vector2(
                cam.LimitLeft + spriteHalfWidth,
                cam.LimitRight - spriteHalfWidth);
        }

        public override void _Input(InputEvent @event) {
            if (@event is InputEventKey e && e.Pressed) {
                if (e.Keycode == Key.Left)
                    _sprite.FlipH = true;
                else if (e.Keycode == Key.Right)
                    _sprite.FlipH = false;
            }
        }

        public override void _Process(double delta) {
            float i = Input.GetAxis("ui_left", "ui_right");
            if (i != 0) {
                Translate(i * Vector2.Right * Speed * (float)delta * 100);
                if (GlobalPosition.X < _mapBounds.X)
                    GlobalPosition = new Vector2(_mapBounds.X, GlobalPosition.Y);
                if (GlobalPosition.X > _mapBounds.Y)
                    GlobalPosition = new Vector2(_mapBounds.Y, GlobalPosition.Y);
            }
        }

    }

}
