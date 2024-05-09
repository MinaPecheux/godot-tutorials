using Godot;

namespace Tutorial34_SceneTransitions
{

	public partial class SceneManager : Node3D
	{
		[Export] private string _targetScene;

		private void _SwitchScene() {
			GetNode<SceneTransition>("/root/SceneTransition").ChangeToScene(_targetScene);
		}
	}

}
