using InventorySystem.Items;
using MEC;
using PlayerRoles;
using SwiftNPCs;
using SwiftNPCs.Core.Management;
using SwiftNPCs.Core.World.AIModules;
using SwiftTD.Interfaces;
using System.IO;
using UnityEngine;

namespace SwiftTD.Core
{
    public class TowerBase(string id, string displayName, RoleTypeId role, ItemType weapon, float price, float range, float turnSpeed, bool headshot) : ISaveAsJSON
    {
        public static readonly string FilePath = Path.Combine(Plugin.PluginFolder, "Towers");

        public string ID = id;
        public string DisplayName = displayName;

        public RoleTypeId Role = role;
        public ItemType Weapon = weapon;

        public float Price = price;
        public float Range = range;
        public float TurnSpeed = turnSpeed;

        public bool Headshot = headshot;

        public string GetFilePath() => FilePath;

        public string GetID() => ID;

        public virtual AIPlayerProfile Spawn(Vector3 pos)
        {
            AIPlayerProfile prof = Utilities.CreateStaticAI(Role, pos, Range);
            prof.WorldPlayer.MovementEngine.LookSpeed = TurnSpeed;
            if (prof.WorldPlayer.ModuleRunner.TryGetModule(out AIFirearmShoot f))
                f.Headshots = Headshot;
            prof.DisplayNickname = DisplayName;

            Timing.CallDelayed(1f, Init);

            return prof;

            void Init()
            {
                ItemBase b = prof.Player.AddItem(Weapon);
                prof.ReferenceHub.inventory.ServerSelectItem(b.ItemSerial);
            }
        }
    }
}
