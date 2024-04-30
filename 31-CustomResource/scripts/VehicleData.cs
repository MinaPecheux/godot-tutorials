using Godot;

namespace Tutorial31_CustomResource
{

    [GlobalClass]
    public partial class VehicleData : Resource {
        [Export] public string name = "";
        [Export] public float speed = 1f;
        [Export] public PackedScene model = null;
    }

}
