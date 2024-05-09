using Godot;
using System.Threading.Tasks;

namespace Tutorial34_SceneTransitions
{

	public partial class SceneTransition : Node
	{
		public enum TransitionMode
		{
			FadeToBlack,
			CrossFade,

			SlideLeft,
			SlideRight,
			SlideUp,
			SlideDown,
		}

		[Export] private float _time = 1.0f;
		[Export] private TransitionMode _mode = TransitionMode.FadeToBlack;
		[Export] private Tween.EaseType _easeType = Tween.EaseType.Out;
		[Export] private Tween.TransitionType _transitionType = Tween.TransitionType.Linear;

		private ColorRect _black;
		private TextureRect _stillRender;

		public override void _Ready() {
			_black = GetNode<ColorRect>("Black");
			_black.Modulate = new Color(0, 0, 0, 0);
			_stillRender = GetNode<TextureRect>("StillRender");
			_stillRender.Modulate = new Color(1, 1, 1, 0);
		}
		
		public async void ChangeToScene(string sceneName) {
			await _TransitionIn();
			GetTree().ChangeSceneToFile($"res://{sceneName}");
			_TransitionOut();
		}

		private async Task _TransitionIn() {
			if (_mode == TransitionMode.FadeToBlack) {
				Tween t = GetTree().CreateTween();
				t.TweenProperty(_black, "modulate:a", 1.0f, _time / 2f);
				await ToSignal(t, Tween.SignalName.Finished);
			} else {
				_stillRender.Texture = ImageTexture.CreateFromImage(GetViewport().GetTexture().GetImage());
				_stillRender.Modulate = new Color(1, 1, 1, 1);
				_stillRender.Position = Vector2.Zero;

				if (_mode == TransitionMode.CrossFade)
					return;

				Vector2 size = _stillRender.Size;
				Tween t = GetTree().CreateTween();
				switch (_mode) {
					case TransitionMode.SlideLeft:
						t.TweenProperty(_stillRender, "position:x", -size.X, _time)
							.SetEase(_easeType).SetTrans(_transitionType);
						break;
					case TransitionMode.SlideRight:
						t.TweenProperty(_stillRender, "position:x", size.X * 2f, _time)
							.SetEase(_easeType).SetTrans(_transitionType);
						break;
					case TransitionMode.SlideUp:
						t.TweenProperty(_stillRender, "position:y", -size.Y, _time)
							.SetEase(_easeType).SetTrans(_transitionType);
						break;
					case TransitionMode.SlideDown:
						t.TweenProperty(_stillRender, "position:y", size.Y * 2f, _time)
							.SetEase(_easeType).SetTrans(_transitionType);
						break;
				}
			}
		}

		private void _TransitionOut() {
			if (
				_mode == TransitionMode.SlideLeft ||
				_mode == TransitionMode.SlideRight ||
				_mode == TransitionMode.SlideUp ||
				_mode == TransitionMode.SlideDown
			)
				return;

			Tween t = GetTree().CreateTween();
			switch (_mode) {
				case TransitionMode.FadeToBlack:
					t.TweenProperty(_black, "modulate:a", 0.0f, _time / 2f);
					break;
				case TransitionMode.CrossFade:
					t.TweenProperty(_stillRender, "modulate:a", 0.0f, _time);
					break;
				default:
					break;
			}
		}

	}
	
}