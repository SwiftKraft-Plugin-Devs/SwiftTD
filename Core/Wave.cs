using MEC;
using SwiftTD.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace SwiftTD.Core
{
    public class Wave(string id, string displayName, params Wave.EnemySegment[] enemySegments) : ISaveAsJSON
    {
        public static readonly string FilePath = Path.Combine(Plugin.PluginFolder, "Waves");

        internal delegate void OnFinish();

        public readonly List<EnemySegment> Enemies = [.. enemySegments];

        internal OnFinish FinishCallback;

        public string ID = id;
        public string DisplayName = displayName;

        public Wave(List<EnemySegment> enemies, string id, string displayName) : this(id, displayName, [.. enemies]) { }

        public Wave(WaveWrapper wrapper) : this(wrapper.ID, wrapper.DisplayName, [])
        {
            foreach (EnemySegmentWrapper wrap in wrapper.Enemies)
                Enemies.Add(new(wrap));
        }

        public void AddSegment(EnemySegment seg) => Enemies.Add(seg);

        public void RemoveSegment(EnemySegment seg) => Enemies.Remove(seg);

        public void RemoveSegment(int seg) => Enemies.RemoveAt(seg);

        public void ClearSegments() => Enemies.Clear();

        public void SpawnAll(SwiftNPCs.Core.Pathing.Path p, Game g)
        {
            Timing.RunCoroutine(Spawn(p, g));
        }

        public IEnumerator<float> Spawn(SwiftNPCs.Core.Pathing.Path p, Game g)
        {
            foreach (EnemySegment enemy in Enemies)
            {
                bool thisFinished = false;

                enemy.FinishCallback -= Finish;
                enemy.FinishCallback += Finish;
                enemy.Spawn(p, g);

                while (!thisFinished)
                    yield return Timing.WaitForOneFrame;

                if (g.Lost)
                    break;

                void Finish()
                {
                    thisFinished = true;
                    enemy.FinishCallback -= Finish;
                }
            }

            FinishCallback?.Invoke();
        }

        public string GetFilePath() => FilePath;

        public string GetID() => ID;

        public static Wave GetWaveFromID(string id) => new(JSONTools.GetSavedFromID<WaveWrapper>(FilePath, id));
        public static bool TryGetWaveFromID(string id, out Wave output)
        {
            output = GetWaveFromID(id);
            return output != null;
        }

        public class EnemySegmentWrapper : IJsonSerializable
        {
            public string EnemyID { get; set; }

            public int Count { get; set; }

            public float Delay { get; set; }
        }

        public class WaveWrapper : IJsonSerializable
        {
            public List<EnemySegmentWrapper> Enemies { get; set; }

            public string ID { get; set; }
            public string DisplayName { get; set; }
        }

        public class EnemySegment : IJsonSerializable
        {
            internal delegate void OnFinish();

            public string EnemyID;

            public int Count;
            public float Delay;

            internal OnFinish FinishCallback;

            private bool Spawning;

            private readonly bool CanSpawn = false;
            private readonly EnemyBase enemy;

            public EnemySegment(EnemySegmentWrapper wrapper) : this(wrapper.EnemyID, wrapper.Count, wrapper.Delay) { }

            public EnemySegment(EnemyBase enemy, int count, float delay) : this(enemy.ID, count, delay)
            {
                if (!CanSpawn)
                {
                    enemy.SaveAsJSON();
                    if (JSONTools.TryGetSavedFromID(EnemyBase.FilePath, EnemyID, out this.enemy))
                        CanSpawn = true;
                }
            }

            public EnemySegment(string enemyId, int count, float delay)
            {
                EnemyID = enemyId;
                Count = count;
                Delay = delay;

                if (JSONTools.TryGetSavedFromID(EnemyBase.FilePath, EnemyID, out enemy))
                    CanSpawn = true;
            }

            public void Spawn(SwiftNPCs.Core.Pathing.Path p, Game g)
            {
                if (Spawning)
                    return;

                if (!CanSpawn)
                {
                    FinishCallback?.Invoke();
                    return;
                }

                Timing.RunCoroutine(SpawnEnemies(p, g));
            }

            public IEnumerator<float> SpawnEnemies(SwiftNPCs.Core.Pathing.Path p, Game g)
            {
                Spawning = true;

                for (int i = 0; i < Count; i++)
                {
                    enemy.Spawn(p, g);
                    yield return Timing.WaitForSeconds(Delay);

                    if (g.Lost)
                        break;
                }

                FinishCallback?.Invoke();
                Spawning = false;
            }
        }
    }
}
