using Godot;
using System;

namespace TowerDefense.Tutorial03_LoadData
{
	
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
			_SetRadius(radius);
		}
		
		public void SetValid(bool isValid)
		{
			Color c = isValid ? _COLOR_VALID : _COLOR_INVALID;
			_spritesMaterial.SetShaderParam("tint", c);
		}
		
		public void SetTowerData(TowerData data)
		{
			GetNode<Sprite>("Base").Texture = data.sprite;
			_SetRadius(data.radius);
		}
		
		private void _SetRadius(float radius)
		{
			this.radius = radius;
			string radiusDisplayPath = "RadiusDisplay";
			ShaderMaterial m = (ShaderMaterial) GetNode<CanvasItem>(radiusDisplayPath).Material;
			m.SetShaderParam("radius", radius / GetNode<MeshInstance2D>(radiusDisplayPath).Scale.x);
		}
	}
	
}
