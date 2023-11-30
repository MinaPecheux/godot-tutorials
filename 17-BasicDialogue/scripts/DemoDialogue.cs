using Godot;
using System;

public partial class DemoDialogue : Node3D
{
	private string[] _availableLanguages;

    public override void _Ready()
    {
		_availableLanguages = TranslationServer.GetLoadedLocales();
        _SetupLanguageSelector();
    }

	private void _SetupLanguageSelector()
	{
		OptionButton selector = GetNode<OptionButton>("UI/Container/LanguageSelector");
		int selected = -1, i = 0;
		foreach (string k in _availableLanguages) {
			if (TranslationServer.GetLocale().Contains(k)) {
				selected = i;
			}
			selector.AddItem(k);
			i++;
		}

		selector.Selected = selected;
	}

	private void _SelectLanguage(int idx)
	{
		TranslationServer.SetLocale(_availableLanguages[idx]);
	}
	
	private async void _StartDialogue()
	{
		GetNode<Star>("Star").Appear();

		await ToSignal(GetTree().CreateTimer(0.8f), Timer.SignalName.Timeout);
		GetNode<DialogueDisplay>("/root/DialogueDisplay").ShowDialogue("star", "conv_star_01");
	}

}
