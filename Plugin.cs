using PlayerRoles;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;
using PluginAPI.Helpers;
using SwiftAPI.API.CustomItems;
using SwiftShops.API;
using SwiftTD.Core;
using SwiftTD.Items;
using System.IO;

namespace SwiftTD
{
    public class Plugin
    {
        public static Plugin Instance;

        public static readonly string PluginFolder = Paths.LocalPlugins.Plugins + "\\" + Name;

        public const string Author = "SwiftKraft";

        public const string Name = "SwiftTD";

        public const string Description = "A tower defense minigame for SCP: SL. ";

        public const string Version = "Alpha v0.0.1";

        [PluginPriority(LoadPriority.Lowest)]
        [PluginEntryPoint(Name, Version, Description, Author)]
        public void Init()
        {
            Instance = this;

            EventManager.RegisterEvents<EventHandler>(this);

            Log.Info("Enemy Files Path: " + EnemyBase.FilePath);
            Log.Info("Wave Files Path: " + Wave.FilePath);

            Directory.CreateDirectory(EnemyBase.FilePath);
            Directory.CreateDirectory(Wave.FilePath);

            SavesManager.LoadSaves();

            CustomItemManager.RegisterItem("TOWER_PISTOL", new TowerCard() { DisplayName = "Pistol Tower", Role = RoleTypeId.Scientist, Weapon = ItemType.GunCOM18 });
            CustomItemManager.RegisterItem("TOWER_RIFLE", new TowerCard() { DisplayName = "Rifle Tower", Role = RoleTypeId.NtfSpecialist, Weapon = ItemType.GunE11SR });
            CustomItemManager.RegisterItem("TOWER_SMG", new TowerCard() { DisplayName = "SMG Tower", Role = RoleTypeId.FacilityGuard, Weapon = ItemType.GunFSP9 });

            ShopProfile profile = new();

            profile.AddItem(new CustomShopItem() { ID = "PISTOLTOWER", Item = CustomItemManager.GetCustomItemWithID("TOWER_PISTOL"), Price = 50f });
            profile.AddItem(new CustomShopItem() { ID = "RIFLETOWER", Item = CustomItemManager.GetCustomItemWithID("TOWER_RIFLE"), Price = 250f });
            profile.AddItem(new CustomShopItem() { ID = "SMGTOWER", Item = CustomItemManager.GetCustomItemWithID("TOWER_SMG"), Price = 150f });

            profile.ID = "TOWERDEFENSE";

            ShopManager.RegisterProfile(profile);

            profile.SetProfileActive(true);

            Log.Info("SwiftTD Loaded! ");
        }
    }
}
