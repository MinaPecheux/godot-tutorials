using Godot;

namespace Tutorial30_AerialCamera
{

	public partial class CursorManager : Node {
		Texture2D _defaultCursor;
		Texture2D _grabCursor;

		public override void _Ready() {
			// load custom cursor images
			_defaultCursor = ResourceLoader.Load<Texture2D>("res://30-AerialCamera/art/sprites/hand_point.png");
			_grabCursor = ResourceLoader.Load<Texture2D>("res://30-AerialCamera/art/sprites/hand_closed.png");

			// set default shape
			Input.SetCustomMouseCursor(_defaultCursor);
		}

		public override void _Input(InputEvent @event) {
			if (@event is InputEventMouseButton e) {
					if (e.Pressed) Input.SetCustomMouseCursor(_grabCursor);
					else Input.SetCustomMouseCursor(_defaultCursor);
				}
		}
	}

}
