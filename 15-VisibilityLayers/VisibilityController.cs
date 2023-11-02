using Godot;
using System;

public partial class VisibilityController : MeshInstance3D
{
	public override void _Ready()
	{
		SetLayerMaskValue(2, false); // remove from layer 2
		SetLayerMaskValue(1, true); // add to layer 1
		SetLayerMaskValue(3, true); // add to layer 3

		GD.Print($"On layer 1? {GetLayerMaskValue(1)}");
		GD.Print($"On layer 2? {GetLayerMaskValue(2)}");
		GD.Print($"On layer 3? {GetLayerMaskValue(3)}");

		GD.Print(Layers);
	}
}
