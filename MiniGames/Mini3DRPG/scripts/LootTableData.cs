using Godot;
using Godot.Collections;

namespace MiniGame_Mini3DRPG
{

    [GlobalClass]
    public partial class LootTableData : Resource
    {
        [Export] public Dictionary<ItemData, float> dropChances;
    }

}
