using InventorySystem.Items;
using MEC;
using PlayerRoles;
using SwiftNPCs;
using SwiftNPCs.Core.Management;
using UnityEngine;

namespace SwiftTD.NPCs
{
    public static class TowerSpawner
    {
        public static AIPlayerProfile SpawnTower(ItemType item, RoleTypeId role, Vector3 pos)
        {
            AIPlayerProfile prof = Utilities.CreateStaticAI(role, pos);

            Timing.CallDelayed(1f, Initialize);

            return prof;

            void Initialize()
            {
                ItemBase b = prof.Player.AddItem(item);
                prof.ReferenceHub.inventory.ServerSelectItem(b.ItemSerial);
            }
        }
    }
}
