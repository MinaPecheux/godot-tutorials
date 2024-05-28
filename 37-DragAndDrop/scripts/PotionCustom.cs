using Godot;
using Godot.Collections;

namespace Tutorial37_DragAndDrop {

	public partial class PotionCustom : TextureRect {

		[Export] private int _shapeID;
		private DragManager _dragManager;

		public override void _Ready() {
			_dragManager = GetNode<DragManager>("/root/DragManager");
		}

		private void _OnGuiInput(InputEvent @event) {
			if (@event is InputEventMouseButton e && e.Pressed
				&& e.ButtonIndex == MouseButton.Left) {
				TextureRect p = new() { Texture = Texture, ExpandMode = ExpandModeEnum.IgnoreSize };
				p.CustomMinimumSize = Vector2.One * 128;
				_dragManager.StartDrag(
					new Dictionary() { { "shape", _shapeID }, { "source", this } },
					p);
			}
		}
	}

}
