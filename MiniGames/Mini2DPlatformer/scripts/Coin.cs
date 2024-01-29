using Godot;

namespace MiniGame_Mini2DPlatformer
{

	public partial class Coin : Area2D
	{
		public async override void _Ready()
		{
			double d = GD.RandRange(0f, 1.2);
			await ToSignal(GetTree().CreateTimer(d), Timer.SignalName.Timeout);
			GetNode<AnimationPlayer>("AnimationPlayer").Play("rotate");
		}
	}

}
