using Godot;
using System.Collections.Generic;

namespace MiniGame_Mini3DRPG
{

    public partial class Orc : Character
    {
        enum AIState {
            Idle,
            ChasingPlayer,
            Attacking
        }

        [Export] private float _fieldOfVision = 5f;
        [Export] private LootTableData _lootTable;

        private AIState _state = AIState.Idle;
        private Character _target;

        public override void _Process(double delta)
        {
            if (_state == AIState.Idle) {
                Character closestTarget = null;
                float closestDistance = Mathf.Inf;
                foreach (Node n in GetTree().GetNodesInGroup("player")) {
                    Character c = (Character) n;
                    float dist = (c.GlobalPosition - GlobalPosition).Length();
                    if (dist < closestDistance) {
                        closestDistance = dist;
                        closestTarget = c;
                    }
                }

                if (closestDistance <= _fieldOfVision) {
                    _target = closestTarget;
                    _state = AIState.ChasingPlayer;
                }
            }
            else {
                float d = (_target.GlobalPosition - GlobalPosition).Length();
                if (_state == AIState.Attacking && d > _attackRange) {
                    _state = AIState.ChasingPlayer;
                    _attackTimer.Stop();
                } else if (d > _fieldOfVision) {
                    _state = AIState.Idle;
                }
            }
        }

        protected override void _Move()
        {
            if (_state == AIState.ChasingPlayer) {
                Vector3 diff = _target.GlobalPosition - GlobalPosition;
                Vector3 direction = diff.Normalized();
                LookAt(_target.GlobalPosition, Vector3.Up);

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

                if (diff.Length() <= _attackRange)
                {
                    _state = AIState.Attacking;
                    _attackTimer.Start();
                }
            }
        }

        private void _Attack()
        {
            _animPlayback.Travel("attack-melee-right");
            bool playerIsDead = _target.TakeHit(_attackDamage);
            if (playerIsDead) {
                _target = null;
                _attackTimer.Stop();
                _state = AIState.Idle;
            }
        }

        protected override void _Die()
        {
            _attackTimer.Stop();
            _GenerateLoot();            
            base._Die();
        }

        private async void _GenerateLoot()
        {
            Dictionary<ItemData, int> loot = new();

            int nItems = GD.RandRange(1, 3);
            for (int i = 0; i < nItems; i++) {
                ItemData d = null;
                float r = GD.Randf();
                float s = 0f;
                foreach (var item in _lootTable.dropChances) {
                    s += item.Value;
                    if (r < s) {
                        d = item.Key;
                        break;
                    }
                }

                int amount = 1;
                if (d.name == "Health Potion" || d.name == "Mana Potion")
                    amount = GD.RandRange(1, 3);
                loot[d] = amount;
            }

            await ToSignal(GetTree().CreateTimer(1f), Timer.SignalName.Timeout);
            GetNode<Loot>("/root/Loot").Set(loot);
        }
    }

}
