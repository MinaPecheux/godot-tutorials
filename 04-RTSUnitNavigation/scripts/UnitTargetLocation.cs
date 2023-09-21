using Godot;
using System;

public partial class UnitTargetLocation : Node3D
{
	private AnimationPlayer _anim;
	
	public override void _Ready()
	{
		_anim = GetNode<AnimationPlayer>("AnimationPlayer");
		
		// initialize to transparent colors everywhere
		var m = (ShaderMaterial) GetNode<MeshInstance3D>("Mesh").GetActiveMaterial(0);
		m.SetShaderParameter("radius", 0.01f);
	}

	public void Click(Vector3 position) {
		GlobalPosition = position + Vector3.Up * 0.02f;
		_anim.Play("UnitTargetLocation");
	}
}
