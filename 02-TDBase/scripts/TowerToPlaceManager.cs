using Godot;
using System;

public class TowerToPlaceManager : Node2D
{
	private static Color _COLOR_VALID = new Color("#4ef544");
	private static Color _COLOR_INVALID = new Color("#f55544");
	
	private ShaderMaterial _spritesMaterial;
	
	// tower parameters
	public float radius = 400f; // in pixels

	public override void _Ready()
	{
		_spritesMaterial = (ShaderMaterial) GetNode<CanvasItem>("Base").Material;

		string radiusDisplayPath = "RadiusDisplay";
		ShaderMaterial m = (ShaderMaterial) GetNode<CanvasItem>(radiusDisplayPath).Material;
		m.SetShaderParam("radius", radius / GetNode<MeshInstance2D>(radiusDisplayPath).Scale.x);
	}
	
	public void SetValid(bool isValid)
	{
		Color c = isValid ? _COLOR_VALID : _COLOR_INVALID;
		_spritesMaterial.SetShaderParam("tint", c);
	}
}
