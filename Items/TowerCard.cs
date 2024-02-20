using InventorySystem;
using InventorySystem.Items;
using PlayerRoles;
using PluginAPI.Core;
using SwiftAPI.API.CustomItems;
using SwiftTD.Core;

namespace SwiftTD.Items
{
    public class TowerCard : CustomItemBase
    {
        public TowerBase Tower;

        public override bool Drop(Player _player, ItemBase _item)
        {
            Tower.Spawn(_player.Position);

            _player.ReferenceHub.inventory.ServerRemoveItem(_item.ItemSerial, _item.PickupDropModel);

            return true;
        }

        public override void Destroy(ushort _itemSerial) { }

        public override void Init(ushort _itemSerial) { }
    }
}
