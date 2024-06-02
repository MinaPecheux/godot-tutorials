using Godot;

namespace MiniGame_Mini3DRPG
{

    public partial class Player : Character
    {
        private Loot _loot;

        private bool _canAttack = true;
        private RayCast3D _attackRaycast;

        public override void _Ready()
        {
            base._Ready();
            _loot = GetNode<Loot>("/root/Loot");
            _attackRaycast = GetNode<RayCast3D>("RayCast3D");
            _attackRaycast.TargetPosition = Vector3.Forward * _attackRange;
        }

        public override void _Input(InputEvent @event)
        {
            if (_loot.Visible) return; // block inputs if in loot panel
            if (Input.IsActionPressed("attack")) _Attack();
        }

        protected override void _Move()
        {
            // Handle Jump.
            if (Input.IsActionJustPressed("jump") && IsOnFloor())
                velocity.Y = _jumpVelocity;

            // Get the input direction and handle the movement/deceleration.
            // As good practice, you should replace UI actions with custom gameplay actions.
            float turnStrength = Input.GetAxis("left", "right");
            float moveStrength = Input.GetAxis("forward", "backwards");

            RotateY(-Mathf.DegToRad(turnStrength * _rotationVelocity));

            Vector3 direction = (Transform.Basis * new Vector3(0, 0, moveStrength)).Normalized();
            if (direction != Vector3.Zero)
            {
                velocity.X = direction.X * _speed;
                velocity.Z = direction.Z * _speed;
            }
            else
            {
                velocity.X = Mathf.MoveToward(Velocity.X, 0, _speed);
                velocity.Z = Mathf.MoveToward(Velocity.Z, 0, _speed);
            }
        }

        private void _Attack()
        {
            if (!_canAttack) return;

            _animPlayback.Travel("attack-melee-right");
            if (_attackRaycast.IsColliding()) {
                ((Character) _attackRaycast.GetCollider()).TakeHit(_attackDamage);
            }

            _canAttack = false;
            _attackTimer.Start();
        }

        private void _OnAttackTimerTimeout()
        {
            _canAttack = true;
        }
    }

}
