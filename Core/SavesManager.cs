using System.Collections.Generic;
using System.IO;

namespace SwiftTD.Core
{
    public static class SavesManager
    {
        public static readonly Dictionary<string, EnemyBase> RegisteredEnemies = [];
        public static readonly Dictionary<string, Wave> RegisteredWaves = [];

        public static void LoadSaves()
        {
            foreach (string file in Directory.EnumerateFiles(EnemyBase.FilePath, "*.json"))
            {
                if (JSONTools.TryJSONConvert(file, out EnemyBase enemy))
                    RegisterEnemy(enemy);
            }

            foreach (string file in Directory.EnumerateFiles(Wave.FilePath, "*.json"))
            {
                if (JSONTools.TryJSONConvert(file, out Wave wave))
                    RegisterWave(wave);
            }
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

        public static void RegisterEnemy(this EnemyBase enemy) { if (!RegisteredEnemies.ContainsKey(enemy.GetID())) RegisteredEnemies.Add(enemy.GetID(), enemy); else RegisteredEnemies[enemy.GetID()] = enemy; }

        public static void RegisterWave(this Wave wave) { if (!RegisteredWaves.ContainsKey(wave.GetID())) RegisteredWaves.Add(wave.GetID(), wave); else RegisteredWaves[wave.GetID()] = wave; }

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
