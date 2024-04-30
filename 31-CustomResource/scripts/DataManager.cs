using Godot;
using Godot.Collections;

namespace Tutorial31_CustomResource
{

	public partial class DataManager : Node3D
	{
		private Label _nameLabel;
		private Label _detailsLabel;
		private Label _debugLabel;
		private Node _showcase;
		private AnimationPlayer _animator;

		[Export] private Array<VehicleData> _availableVehicles;
		private VehicleData _data = null;

		public override void _Ready()
		{
			_nameLabel = GetNode<Label>("%Name");
			_detailsLabel = GetNode<Label>("%Details");
			_debugLabel = GetNode<Label>("%Debug");
			_showcase = GetNode("SHOWCASE");
			_animator = GetNode<AnimationPlayer>("AnimationPlayer");
		}

		private void _Randomise() {
			Array<VehicleData> vehicles = _availableVehicles.Duplicate();
			if (_data != null)
				vehicles.Remove(_data);
			_LoadData(vehicles.PickRandom());
		}

		private void _LoadData(VehicleData data) {
			_data = data;
			_nameLabel.Text = data.name;
			_detailsLabel.Text = $"Speed: {data.speed}";
			if (_showcase.GetChildCount() > 0)
				_showcase.GetChild(0).QueueFree();
			_showcase.AddChild(data.model.Instantiate());
			_animator.Play("spawn");
		}

		private void _Debug(string text, bool error = false) {
			if (!error) {
				_debugLabel.Set("theme_override_colors/font_color", "#f5f6fa");
				_debugLabel.Set("theme_override_colors/font_outline_color", "#00a8ff");
			} else {
				_debugLabel.Set("theme_override_colors/font_color", "#f5f6fa");
				_debugLabel.Set("theme_override_colors/font_outline_color", "#e84118");
			}
			_debugLabel.Text = text;
			_debugLabel.PivotOffset = _debugLabel.Size * 0.5f;
			_debugLabel.RotationDegrees = GD.RandRange(5, 15) * (GD.Randf() < 0.5f ? -1f : 1f);
			_debugLabel.GetNode<AnimationPlayer>("AnimationPlayer").Play("show-debug");
		}

		private void _Load() {
			string fileName = "res://31-CustomResource/data.tres";
			if (ResourceLoader.Exists(fileName)) {
				_Debug("LOADED!");
				_LoadData(ResourceLoader.Load<VehicleData>(
					fileName,
					null,
					ResourceLoader.CacheMode.Ignore));
			} else {
				_Debug("NO DATA", true);
			}
		}

		private void _Save() {
			string fileName = "res://31-CustomResource/data.tres";
			ResourceSaver.Save(_data, fileName);
			_Debug("SAVED!");
		}

	}

}
