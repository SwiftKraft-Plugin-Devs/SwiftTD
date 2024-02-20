using PluginAPI.Core;
using SwiftAPI.API.CustomItems;
using SwiftShops.API;
using SwiftTD.Items;
using System.Collections.Generic;
using System.IO;

namespace SwiftTD.Core
{
    public static class SavesManager
    {
        public static readonly Dictionary<string, EnemyBase> RegisteredEnemies = [];
        public static readonly Dictionary<string, TowerBase> RegisteredTowers = [];
        public static readonly Dictionary<string, Wave> RegisteredWaves = [];

        public static ShopProfile Shop = new();

        public static void LoadSaves()
        {
            foreach (string file in Directory.EnumerateFiles(EnemyBase.FilePath, "*.json"))
            {
                if (JSONTools.TryJSONConvert(file, out EnemyBase enemy))
                    RegisterEnemy(enemy);
            }

            foreach (string file in Directory.EnumerateFiles(TowerBase.FilePath, "*.json"))
            {
                if (JSONTools.TryJSONConvert(file, out TowerBase tower))
                    RegisterTower(tower);
            }

            foreach (string file in Directory.EnumerateFiles(Wave.FilePath, "*.json"))
            {
                if (JSONTools.TryJSONConvert(file, out Wave wave))
                    RegisterWave(wave);
            }

            Shop.ID = "TOWERDEFENSE";
            Shop.DisplayName = "Tower Defense Shop";

            ShopManager.RegisterProfile(Shop);

            Shop.SetProfileActive(true);
        }

        public static TowerBase GetTower(string id)
        {
            if (RegisteredTowers.ContainsKey(id))
                return RegisteredTowers[id];
            return null;
        }

        public static bool TryGetTower(string id, out TowerBase tower)
        {
            tower = GetTower(id);
            return tower != null;
        }

        public static EnemyBase GetEnemy(string id)
        {
            if (RegisteredEnemies.ContainsKey(id))
                return RegisteredEnemies[id];
            return null;
        }

        public static bool TryGetEnemy(string id, out EnemyBase enemy)
        {
            enemy = GetEnemy(id);
            return enemy != null;
        }

        public static Wave GetWave(string id)
        {
            if (RegisteredWaves.ContainsKey(id))
                return RegisteredWaves[id];
            return null;
        }

        public static bool TryGetWave(string id, out Wave wave)
        {
            wave = GetWave(id);
            return wave != null;
        }

        public static void RegisterTower(this TowerBase tower)
        {
            if (!RegisteredTowers.ContainsKey(tower.GetID()))
            {
                RegisteredTowers.Add(tower.GetID(), tower);
                TowerCard card = new() { Tower = tower, DisplayName = tower.DisplayName };
                CustomItemManager.RegisterItem("TD." + tower.GetID(), card);
                Shop.AddItem(new CustomShopItem() { ID = tower.GetID(), Price = tower.Price, Item = card });
            }
            else
                Log.Error("Failed to register tower: " + tower.DisplayName);
        }

        public static void RegisterEnemy(this EnemyBase enemy) { if (!RegisteredEnemies.ContainsKey(enemy.GetID())) RegisteredEnemies.Add(enemy.GetID(), enemy); else RegisteredEnemies[enemy.GetID()] = enemy; }

        public static void RegisterWave(this Wave wave) { if (!RegisteredWaves.ContainsKey(wave.GetID())) RegisteredWaves.Add(wave.GetID(), wave); else RegisteredWaves[wave.GetID()] = wave; }

        public static void SaveTower(this TowerBase tower)
        {
            RegisterTower(tower);

            tower.SaveAsJSON();
        }

        public static void SaveEnemy(this EnemyBase enemy)
        {
            RegisterEnemy(enemy);

            enemy.SaveAsJSON();
        }

        public static void SaveWave(this Wave wave)
        {
            RegisterWave(wave);

            wave.SaveAsJSON();
        }
    }
}
