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
            Log.Info("Tower Files Path: " + TowerBase.FilePath);
            Log.Info("Wave Files Path: " + Wave.FilePath);

            Directory.CreateDirectory(EnemyBase.FilePath);
            Directory.CreateDirectory(TowerBase.FilePath);
            Directory.CreateDirectory(Wave.FilePath);

            SavesManager.LoadSaves();

            Log.Info("SwiftTD Loaded! ");
        }
    }
}
