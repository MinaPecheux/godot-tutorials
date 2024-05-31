using Godot;

namespace Tutorial38_ToggleButtons {

	public partial class SettingsManager : Control {

		private void _ToggleLanguage(bool toggledOn, string languageCode) {
			if (toggledOn)
				TranslationServer.SetLocale(languageCode);
		}

	}

}
