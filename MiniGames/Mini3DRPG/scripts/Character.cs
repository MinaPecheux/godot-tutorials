using Godot;

namespace MiniGame_Mini3DRPG
{

    public partial class Character : CharacterBody3D
    {
        #region Movement
        [Export] protected float _speed = 2.0f;
        [Export] protected float _jumpVelocity = 3.2f;
        [Export] protected float _rotationVelocity = 3.5f;
        [Export] public Vector3 velocity;
        protected float _gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
        #endregion

        protected AnimationTree _anim;
        protected AnimationNodeStateMachinePlayback _animPlayback;

        #region Attack
        [Export] protected int _attackDamage = 5;
        [Export] protected float _attackRate = 1f;
        [Export] protected float _attackRange = 0.5f;
        protected Timer _attackTimer;
        #endregion

        #region Health
        [Export] private int _maxHealth = 100;
        [Export] private string _healthBarPath;
        private ProgressBar _healthBar;
        private int _curHealth;
        #endregion

        public override void _Ready()
        {
            _anim = GetNode<AnimationTree>("AnimationTree");
            _anim.Active = true;
            _animPlayback = (AnimationNodeStateMachinePlayback) _anim.Get("parameters/playback");

            _healthBar = GetNode<ProgressBar>(_healthBarPath);
            _curHealth = _maxHealth;
            _healthBar.MaxValue = _maxHealth;
            _healthBar.Value = _curHealth;

            _attackTimer = GetNode<Timer>("AttackTimer");
            _attackTimer.WaitTime = _attackRate;
        }

        public override void _PhysicsProcess(double delta)
        {
            velocity = Velocity;

            // Add the gravity.
            if (!IsOnFloor())
                velocity.Y -= _gravity * (float)delta;

            _Move();

            Velocity = velocity;
            MoveAndSlide();
        }

        public bool TakeHit(int dmg)
        {
            _animPlayback.Travel("emote-no");
            _curHealth -= dmg;
            _healthBar.Value = _curHealth;
            if (_curHealth <= 0) {
                _Die();
                return true;
            }
            return false;
        }

        protected virtual void _Die()
        {
            _animPlayback.Travel("die");
            SetProcess(false);
            SetPhysicsProcess(false);
        }

        protected virtual void _Move() {}

    }

}
