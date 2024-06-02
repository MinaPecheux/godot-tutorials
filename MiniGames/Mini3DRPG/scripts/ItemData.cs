using Godot;

namespace MiniGame_Mini3DRPG
{

    public enum ItemRarity {
        Common,
        Unusual,
        Rare,
        Epic
    }

    [GlobalClass]
    public partial class ItemData : Resource
    {
        [Export] public string name;
        [Export] public string description;
        [Export] public int value;
        [Export] public ItemRarity rarity;

        [Export] public Texture2D icon;

        public string GetFullDescription()
        {
            string color = rarity switch {
                ItemRarity.Unusual => "green",
                ItemRarity.Rare => "cyan",
                ItemRarity.Epic => "orange",
                _ => "white"
            };
            string s = $"[font_size=16][b][color={color}]{name}";
            s += "[/color][/b][/font_size]\n";
            s += $"{value} [img=16]res://MiniGames/Mini3DRPG/art/icons/coin.png[/img]\n";
            s += description;
            return s;
        }
    }

}
