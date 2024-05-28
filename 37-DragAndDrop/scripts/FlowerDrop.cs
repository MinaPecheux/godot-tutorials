using Godot;
using Godot.Collections;

namespace Tutorial37_DragAndDrop {

	public partial class FlowerDrop : TextureRect {

		[Export] private string _color;

		public override bool _CanDropData(Vector2 atPosition, Variant data) {
			if (data.VariantType != Variant.Type.Dictionary) return false;
			Dictionary d = data.AsGodotDictionary();
			return d.ContainsKey("shape") && d.ContainsKey("source");
		}

		public override void _DropData(Vector2 atPosition, Variant data) {
			Dictionary d = data.AsGodotDictionary();
			string i = System.String.Format("{0:D2}", d["shape"].AsInt32());
			Texture2D tex = GD.Load<Texture2D>($"res://37-DragAndDrop/art/{_color}/{i}.png");
			((TextureRect) d["source"].AsGodotObject()).Texture = tex;
		}

	}

}
