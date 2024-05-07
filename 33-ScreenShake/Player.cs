/*
 * This script is an extended version of the one written in the
 * previous tutorial of the series (see the folder: /32-Random2DObstacles/scripts).
 *
 * For it to work, make sure you have a Camera2D node (named "Camera2D") in your
 * main demo scene, and that the root node of this demo scene is called "Root" :)
 *
 * (For more info, see the tutorial: )
 */
using Godot;

namespace Tutorial33_ScreenShake
{

    public partial class Player : CharacterBody2D {
        public const float Speed = 600.0f;

        private Camera2D _cam;
        private FastNoiseLite _noise;

        private Vector2 _camBasePosition;
        private const float ShakeTime = 1.0f;
        private const float ShakeAmount = 5000.0f;
        private float _screenShakeDelay;

        public override void _Ready() {
            _cam = GetNode<Camera2D>("/root/Root/Camera2D");
            _noise = new();
        }

        public override void _PhysicsProcess(double delta) {
            Vector2 velocity = Velocity;

            float direction = Input.GetAxis("ui_left", "ui_right");
            if (!Mathf.IsZeroApprox(direction)) {
                velocity.X = direction * Speed;
            } else {
                velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            }

            Velocity = velocity;
            MoveAndSlide();
        }

        private void _OnArea2DBodyEntered(Node2D body) {
            if (body.IsInGroup("asteroids")) {
                _ScreenShake();
                body.QueueFree();
            }
        }

        private void _ScreenShake() {
            _camBasePosition = _cam.GlobalPosition;
            _screenShakeDelay = ShakeTime;
        }

        public override void _Process(double delta) {
            if (_screenShakeDelay > 0f) {
                _cam.GlobalPosition = _camBasePosition
                    + new Vector2(_GetNoise(0), _GetNoise(1));
                _screenShakeDelay -= (float) delta;
            }
        }

        private float _GetNoise(int seed) {
            _noise.Seed = seed;
            return _noise.GetNoise1D(
                GD.Randf() * _screenShakeDelay) * ShakeAmount;
        }
    }

}
