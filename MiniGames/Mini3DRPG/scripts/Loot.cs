using Godot;
using System.Collections.Generic;

namespace MiniGame_Mini3DRPG
{

    public partial class Loot : CenterContainer
    {
        private Inventory _inventory;
        private Control _slotsContainer;
        private Control _infoPanel;
        private RichTextLabel _infoPanelText;

        private List<(ItemData, int)> _loot = new();
        private int _nConsumedItems;
        
        public override void _Ready()
        {
            _inventory = GetNode<Inventory>("/root/Inventory");
            _slotsContainer = GetNode<Control>("PanelContainer/VBoxContainer");
            _infoPanel = GetNode<Control>("Info");
            _infoPanelText = GetNode<RichTextLabel>("Info/PanelContainer/RichTextLabel");
            _infoPanel.Hide();

            int i = 0;
            foreach (Node n in _slotsContainer.GetChildren()) {
                Control cell = (Control) n;
                _SetupOnCellGuiInput(cell, i);
                _SetupOnCellMouseEntered(cell, i);
                cell.MouseExited += _OnCellMouseExited;
                i++;
            }
            Hide();
        }

        private void _SetupOnCellGuiInput(Control cell, int i) {
            cell.GuiInput += (InputEvent e) => _OnCellGuiInput(e, i);
        }

        private void _SetupOnCellMouseEntered(Control cell, int i) {
            cell.MouseEntered += () => _OnCellMouseEntered(i);
        }

        public void Set(Dictionary<ItemData, int> loot)
        {
            _loot.Clear();
            _nConsumedItems = 0;

            int i = 0;
            foreach (var p in loot) {
                _loot.Add((p.Key, p.Value));

                Control cell = _slotsContainer.GetChild<Control>(i);
                string c = p.Value > 1 ? $"{p.Value}" : "";
                cell.GetNode<TextureRect>("Icon").Texture = p.Key.icon;
                cell.GetNode<Label>("MarginContainer/Count").Text = c;
                cell.Show();
                i++;
            }

            while (i < 3) {
                _slotsContainer.GetChild<Control>(i).Hide();
                i++;
            }

            Show();
        }

        private void _OnCellGuiInput(InputEvent @event, int i)
        {
            if (@event is InputEventMouseButton e && e.Pressed
                && e.ButtonIndex == MouseButton.Left) {
                (ItemData d, int amount) = _loot[i];
                for (int k = 0; k < amount; k++) {
                    if (!_inventory.AddItem(d)) {
                        return;
                    }
                }
                _slotsContainer.GetChild<Control>(i).Hide();
                _nConsumedItems++;

                if (_nConsumedItems == _loot.Count)
                    Hide();
            }
        }

        private void _OnCellMouseEntered(int i)
        {
            if (i > _loot.Count) return;
            _infoPanelText.Text = _loot[i].Item1.GetFullDescription();
            _infoPanel.Show();
        }

        private void _OnCellMouseExited()
        {
            _infoPanel.Hide();
        }
    }

}
