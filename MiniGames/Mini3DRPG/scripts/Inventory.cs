using Godot;
using System.Collections.Generic;

namespace MiniGame_Mini3DRPG
{

    public partial class Inventory : Control
    {
        private Dictionary<ItemData, int> _items = new();
        private Dictionary<ItemData, int> _itemPositions = new();

        private Control _itemsContainer;
        private Control _itemsDetail;
        private RichTextLabel _itemsDetailText;
        private int _nSlots;

        public override void _Ready()
        {
            _itemsContainer = GetNode<Control>("HBoxContainer");
            _itemsDetail = GetNode<Control>("PanelContainer");
            _itemsDetailText = GetNode<RichTextLabel>("PanelContainer/RichTextLabel");
            _itemsDetail.Hide();
            _nSlots = _itemsContainer.GetChildCount();

            int i = 0;
            foreach (Node n in _itemsContainer.GetChildren()) {
                Control cell = (Control) n;
                _SetupOnCellGuiInput(cell, i);
                _SetupOnCellMouseEntered(cell, i);
                cell.MouseExited += _OnCellMouseExited;
                i++;
            }
        }

        private void _SetupOnCellGuiInput(Control cell, int i) {
            cell.GuiInput += (InputEvent e) => _OnCellGuiInput(e, i);
        }

        private void _SetupOnCellMouseEntered(Control cell, int i) {
            cell.MouseEntered += () => _OnCellMouseEntered(i);
        }

        public bool AddItem(ItemData type)
        {
            if (!_items.ContainsKey(type)) {
                if (_items.Count == _nSlots) // no more space!
                    return false;
                _itemPositions[type] = _items.Count;
                _items[type] = 1;
                Node cell = _itemsContainer.GetChild(_itemPositions[type]);
                cell.GetNode<TextureRect>("Icon").Texture = type.icon;
                cell.GetNode<Label>("MarginContainer/Count").Text = "";
            }
            else {
                _items[type]++;
                _UpdateCellCount(type);
            }
            return true;
        }

        public void RemoveItem(ItemData type)
        {
            if (!_items.ContainsKey(type)) return;
            _items[type]--;
            _UpdateCellCount(type);
            if (_items[type] == 0) {
                Node cell = _itemsContainer.GetChild(_itemPositions[type]);
                cell.GetNode<TextureRect>("Icon").Texture = null;
                cell.GetNode<Label>("MarginContainer/Count").Text = "";
                _items.Remove(type);
                _itemPositions.Remove(type);
            }
        }

        private void _UpdateCellCount(ItemData type)
        {
            string c = _items[type] > 1 ? $"{_items[type]}" : "";
            Node cell = _itemsContainer.GetChild(_itemPositions[type]);
            cell.GetNode<Label>("MarginContainer/Count").Text = c;
        }

        private void _OnCellGuiInput(InputEvent @event, int i)
        {
            if (@event is InputEventMouseButton e && e.Pressed
                && e.ButtonIndex == MouseButton.Right) {
                foreach (KeyValuePair<ItemData, int> p in _itemPositions)
                    if (p.Value == i) {
                        RemoveItem(p.Key);
                        // if removed all instances, hide details
                        if (!_items.ContainsKey(p.Key))
                            _itemsDetail.Hide();
                    }
                }
        }

        private void _OnCellMouseEntered(int i)
        {
            foreach (KeyValuePair<ItemData, int> p in _itemPositions)
                if (p.Value == i) {
                    _itemsDetailText.Text = p.Key.GetFullDescription();
                    _itemsDetail.Show();
                }
        }

        private void _OnCellMouseExited()
        {
            _itemsDetail.Hide();
        }
    }

}
