using Godot;
using System;

public partial class ButtonManager : StaticBody3D
{

    private void _OnInputEvent(Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shape_idx)
    {
        if (@event is InputEventMouseButton mouseEvent) {
            Events.Emit_RedButtonClicked();
        }
    }

}
