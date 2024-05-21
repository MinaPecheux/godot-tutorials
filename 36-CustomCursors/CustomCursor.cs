using Godot;

namespace Tutorial36_CustomCursors {

	public partial class CustomCursor : Node {
		Texture2D _defaultCursor;
		Texture2D _grabCursor;

		public override void _Ready() {
			_defaultCursor = ResourceLoader.Load<Texture2D>("res://36-CustomCursors/art/cursors/default.png");
			_grabCursor = ResourceLoader.Load<Texture2D>("res://36-CustomCursors/art/cursors/grab.png");

			Input.SetCustomMouseCursor(_defaultCursor);
			Input.SetCustomMouseCursor(
				ResourceLoader.Load<Texture2D>("res://36-CustomCursors/art/cursors/wait.png"),
				Input.CursorShape.Wait);
			Input.SetCustomMouseCursor(
				ResourceLoader.Load<Texture2D>("res://36-CustomCursors/art/cursors/drag.png"),
				Input.CursorShape.Drag);
			Input.SetCustomMouseCursor(
				ResourceLoader.Load<Texture2D>("res://36-CustomCursors/art/cursors/forbidden.png"),
				Input.CursorShape.Forbidden);
			Input.SetCustomMouseCursor(
				ResourceLoader.Load<Texture2D>("res://36-CustomCursors/art/cursors/i-beam.png"),
				Input.CursorShape.Ibeam);
		}

		public override void _Input(InputEvent @event) {
			if (@event is InputEventMouseButton e && e.ButtonIndex == MouseButton.Right) {
					if (e.Pressed) Input.SetCustomMouseCursor(_grabCursor);
					else Input.SetCustomMouseCursor(_defaultCursor);
				}
		}
	}

}
