using Godot;
using System;

public partial class GameManager : Node3D
{

    [Export] private PackedScene _targetPointPrefab;
    private Camera3D _cam;
    private Player _player;

    private bool _rightClickedThisFrame = false;
    private Vector2 _rightClickMousePos;

    public override void _Ready()
    {
        _cam = GetNode<Camera3D>("Camera3D");
        _player = GetNode<Player>("Soldier");
    }

    public override void _Input(InputEvent @event)
    {
        if (
            @event is InputEventMouseButton e &&
            e.Pressed && e.ButtonIndex == MouseButton.Right
        ) {
            _rightClickedThisFrame = true;
            _rightClickMousePos = e.Position;
        } else {
            _rightClickedThisFrame = false;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_rightClickedThisFrame) {
            var _from = _cam.ProjectRayOrigin(_rightClickMousePos);
            var to = _from + _cam.ProjectRayNormal(_rightClickMousePos) * 1000f;

            var spaceState = GetWorld3D().DirectSpaceState;
            var query = PhysicsRayQueryParameters3D.Create(
                _from, to, 2 /* /!\ bitmask */
            );
            var result = spaceState.IntersectRay(query);
            if (result.Count > 0) {
                Node3D p = _targetPointPrefab.Instantiate<Node3D>();
                AddChild(p);
                p.GlobalPosition = result["position"].AsVector3();

                _player.SetTargetPosition(result["position"].AsVector3());
            }
        }
    }

}
