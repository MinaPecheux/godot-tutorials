using Godot;
using Godot.Collections;

namespace Tutorial37_DragAndDrop {

	public partial class Potion : TextureRect {

		[Export] private int _shapeID;

		public override void _Ready() {
			string i = System.String.Format("{0:D2}", _shapeID);
			Texture = GD.Load<Texture2D>($"res://37-DragAndDrop/art/white/{i}.png");
		}

		public override Variant _GetDragData(Vector2 atPosition) {
			TextureRect p = new() { Texture = Texture, ExpandMode = ExpandModeEnum.IgnoreSize };
			p.CustomMinimumSize = Vector2.One * 128;
			SetDragPreview(p);
			return new Dictionary() {
				{ "shape", _shapeID },
				{ "source", this } };
		}	

	}

}
