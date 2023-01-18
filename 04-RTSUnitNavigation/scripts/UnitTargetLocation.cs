using Godot;
using System;

public class UnitTargetLocation : Spatial
{
	private AnimationPlayer _anim;
	
	public override void _Ready()
	{
		_anim = GetNode<AnimationPlayer>("AnimationPlayer");
		
		// initialize to transparent colors everywhere
		ShaderMaterial m = (ShaderMaterial) GetNode<CSGMesh>("Mesh").Material;
		m.SetShaderParam("radius", 0.01f);
	}

	public void Click(Vector3 position) {
		GlobalTranslation = position + Vector3.Up * 0.02f;
		_anim.Play("UnitTargetLocation");
	}
}
