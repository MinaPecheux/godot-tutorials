using Godot;
using System;

namespace Tutorial24_2DNav
{

    public partial class Unit : CharacterBody2D
    {
        private NavigationAgent2D _agent;
        private float _speed = 300f;

        public override void _Ready()
        {
            _agent = GetNode<NavigationAgent2D>("NavigationAgent2D");
        }

        public override void _Input(InputEvent @event)
        {
            if (
                @event is InputEventMouseButton e &&
                !e.Pressed &&
                e.ButtonIndex == MouseButton.Right
            ) {
                var map = GetWorld2D().NavigationMap;
                var p = NavigationServer2D.MapGetClosestPoint(map, e.Position);
                _agent.TargetPosition = p;
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            if (_agent.IsNavigationFinished())
                return;

            LookAt(_agent.GetNextPathPosition());
            Vector2 diff = _agent.GetNextPathPosition() - GlobalPosition;
            Vector2 dir = diff.Normalized();
            Velocity = dir * _speed;
            MoveAndSlide();
        }

    }

}
