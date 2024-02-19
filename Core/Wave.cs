using MEC;
using SwiftTD.Interfaces;
using System;
using System.Collections.Generic;

namespace SwiftTD.Core
{
    [Serializable]
    public class Wave : ISaveAsJSON
    {
        public static readonly string FilePath = Plugin.PluginFolder + "\\Waves\\";

        internal delegate void OnFinish();

        internal readonly List<EnemySegment> Enemies = [];

        public SegmentList SegmentIDs = new();

        internal OnFinish FinishCallback;

        public string ID;
        public string DisplayName;

        public Wave(string id, string displayName, params string[] enemySegments)
        {
            DisplayName = displayName;
            ID = id;

            SegmentIDs = new(enemySegments);

            foreach (string i in SegmentIDs.SegmentIDs)
            {
                if (JSONTools.TryGetSavedFromID(EnemySegment.FilePath, i, out EnemySegment seg))
                    Enemies.Add(seg);
            }
        }

        public Wave(string id, string displayName, params EnemySegment[] enemySegments)
        {
            DisplayName = displayName;
            ID = id;

            foreach (EnemySegment i in enemySegments)
            {
                i.SaveAsJSON();

                if (JSONTools.TryGetSavedFromID(EnemySegment.FilePath, i.GetID(), out EnemySegment seg))
                {
                    SegmentIDs.Add(i.GetID());
                    Enemies.Add(seg);
                }
            }
        }

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

        [Serializable]
        public class SegmentList : IJsonSerializable
        {
            public readonly List<string> SegmentIDs = [];

            public SegmentList() { }

            public SegmentList(params string[] ids)
            {
                SegmentIDs = [.. ids];
            }

            public void Add(string id) => SegmentIDs.Add(id);
        }

        [Serializable]
        public class EnemySegment : ISaveAsJSON
        {
            public static readonly string FilePath = Plugin.PluginFolder + "\\Segments\\";

            internal delegate void OnFinish();

            public string ID;
            public string DisplayName;

            public string EnemyID;

            public int Count;
            public float Delay;

            internal OnFinish FinishCallback;

            private bool Spawning;

            private readonly bool CanSpawn = false;
            private readonly EnemyBase enemy;

            public EnemySegment(string id, string displayName, EnemyBase enemy, int count, float delay) : this(id, displayName, enemy.ID, count, delay)
            {
                if (!CanSpawn)
                {
                    enemy.SaveAsJSON();
                    if (JSONTools.TryGetSavedFromID(EnemyBase.FilePath, EnemyID, out this.enemy))
                        CanSpawn = true;
                }
            }

            public EnemySegment(string id, string displayName, string enemyId, int count, float delay)
            {
                ID = id;
                DisplayName = displayName;

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

            public string GetFilePath() => FilePath;

            public string GetID() => ID;
        }
    }
}
