using Godot;

namespace Tutorial37_DragAndDrop {

	public class Drag {
		public Variant data;
		public Control preview;

		public Drag(Variant d, Control p) {
			data = d; preview = p;
		}
	}

	public partial class DragManager : Node {
		private Drag _drag;

		public void StartDrag(Variant data, Control preview) {
			_drag = new(data, preview);
			GetNode("/root/Root").AddChild(_drag.preview);
			_drag.preview.MouseFilter = Control.MouseFilterEnum.Ignore;
			_drag.preview.ZIndex = 999;
			_drag.preview.GlobalPosition = GetViewport().GetMousePosition();
		}

		public void FinishDrag(System.Action<Variant> onSuccess = null) {
			onSuccess?.Invoke(_drag.data);
			StopDrag();
		}

		public void StopDrag() {
			_drag.preview.QueueFree();
			_drag = null;
		}

		public override void _Input(InputEvent @event) {
			if (_drag != null) {
				if (@event is InputEventMouseButton e2 && e2.Pressed &&
					e2.ButtonIndex == MouseButton.Right)
					StopDrag();
				else if (@event is InputEventMouseMotion e)
					_drag.preview.GlobalPosition = e.GlobalPosition;
			}
		}

	}

}
