using Godot;

namespace Tutorial28_EndlessLevel {

    public partial class Obstacle : Area3D {

        private void _OnPlayerCollides(Node3D body) {
            if (body.Name == "Player") {
                // player just hit the obstacle!
                GD.Print("hit!");
            }
        }

        private void _OnPlayerAvoids(Node3D body) {
            if (body.Name == "Player") {
                // player just avoided the obstacle
                GD.Print("dodged!");
            }
        }
    }

}
