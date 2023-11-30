using Godot;
using System;
using System.Collections.Generic;

public partial class DialogueDisplay : PanelContainer
{
	private static Dictionary<string, Texture2D> _LOADED_ICONS
		= new Dictionary<string, Texture2D>();

	private static Dictionary<string, string[]> _CONVERSATIONS
		= new Dictionary<string, string[]>()
		{
			{ "conv_star_01", new string[] {
				"greeting_01",
				"greeting_02",
			} }
		};

	[Export] private AudioStreamPlayer _audio;
	[Export] private TextureRect _icon;
	[Export] private Label _label;

	private int _conversationSentenceIdx;
	private string _conversationID;

	public override void _Ready()
	{
		Hide();
	}

	public void ShowDialogue(string npc, string conversationID)
	{
		Show();
		Texture2D tex;
		if (!_LOADED_ICONS.TryGetValue(npc, out tex)) {
			tex = GD.Load<Texture2D>($"res://17-BasicDialogue/art/icons/{npc}.png");
			_LOADED_ICONS[npc] = tex;
		}
		_icon.Texture = tex;
		_ShowDialogueText(_CONVERSATIONS[conversationID][0]);

		_conversationID = conversationID;
		_conversationSentenceIdx = 1; // for next time!
	}

	private async void _ShowDialogueText(string key)
	{
		_audio.Stream = GD.Load<AudioStream>($"res://17-BasicDialogue/art/audio/{key}-en.wav");
		_audio.Play();

		string text = Tr(key);

		float appearTime = 1.5f; // in seconds
		float appearSpeed = appearTime / (float)(text.Length - 1);

		_label.Text = "";
		foreach (char c in text) {
			_label.Text += c;
			await ToSignal(GetTree().CreateTimer(appearSpeed), Timer.SignalName.Timeout);
		}
	}

	private void _GetNextDialogue()
	{
		string[] sentences = _CONVERSATIONS[_conversationID];
		if (_conversationSentenceIdx < sentences.Length)
			_ShowDialogueText(sentences[_conversationSentenceIdx++]);
		else
			Hide();
	}
}
