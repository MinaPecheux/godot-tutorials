using Godot;

namespace Tutorial28_EndlessLevel {

    public partial class GameManager : Node3D {
        private const float _groupLength = 3f;

        [Export] private Node3D _scene;
        [Export] private PackedScene[] _groups;
        private float _levelSpeed = 1f;

        private float _currentGroupProgress;

        public override void _Ready() {
            _currentGroupProgress = 0;

            // setup start-up platforms (all forced to empty
            // except the last one, to give the players some
            // time to prepare)
            for (int i = -2; i < 3; i++) {
                _InstantiateGroup(i, 0);
            }
            _InstantiateGroup(3);
        }

        private void _InstantiateGroup(int offset = 3, int type = -1) {
            Node3D group = _groups[(type == -1)
                ? GD.RandRange(0, _groups.Length - 1)
                : type].Instantiate<Node3D>();
            _scene.AddChild(group);
            group.GlobalPosition = Vector3.Right * offset * _groupLength;
        }

        public override void _Process(double delta) {
            float progress = _levelSpeed * (float)delta;
            foreach (Node n in _scene.GetChildren())
                ((Node3D) n).Translate(Vector3.Left * progress);
            _currentGroupProgress += progress;
            if (Mathf.IsEqualApprox(_currentGroupProgress, _groupLength, 0.1f)) {
                _InstantiateGroup();
                _scene.GetChild(0).QueueFree();
                _currentGroupProgress = 0;
            }
        }
    }

}
