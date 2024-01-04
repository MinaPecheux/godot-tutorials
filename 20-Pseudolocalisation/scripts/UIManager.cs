using Godot;
using System;

namespace Tutorial20_Pseudolocalization
{

	public partial class UIManager : Control
	{

		[Export] private Control _skillDescriptionPanel;
		[Export] private Label _skillNameLabel;
		[Export] private Label _skillDescriptionLabel;

		[Export] private Control _menuPanel;

		public override void _Ready()
		{
			_HideSkillDescription();
			_HideMenu();
		}
		
		private void _ShowSkillDescription(string skillName)
		{
			_skillNameLabel.Text = $"KEY_{skillName}_NAME";
			_skillDescriptionLabel.Text = $"KEY_{skillName}_DESC";
			_skillDescriptionPanel.Show();
		}

		private void _HideSkillDescription()
		{
			_skillDescriptionPanel.Hide();
		}

		private void _ShowMenu()
		{
			_menuPanel.Show();
			_menuPanel.GetNode<AnimationPlayer>("AnimationPlayer").Play("show-menu");
		}

		private void _HideMenu()
		{
			_menuPanel.Hide();
		}

	}

}
