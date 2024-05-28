using Godot;
using Godot.Collections;

namespace Tutorial37_DragAndDrop {

	public partial class FlowerDropCustom : TextureRect {

		[Export] private string _color;
		private DragManager _dragManager;

		public override void _Ready() {
			_dragManager = GetNode<DragManager>("/root/DragManager");
		}

		private void _OnGuiInput(InputEvent @event) {
			if (@event is InputEventMouseButton e && !e.Pressed
				&& e.ButtonIndex == MouseButton.Left) {
				_dragManager.FinishDrag((Variant data) => {
					Dictionary d = data.AsGodotDictionary();
					string i = System.String.Format("{0:D2}", d["shape"].AsInt32());
					Texture2D tex = GD.Load<Texture2D>($"res://37-DragAndDrop/art/{_color}/{i}.png");
					((TextureRect) d["source"].AsGodotObject()).Texture = tex;
				});
			}
		}

	}

}
