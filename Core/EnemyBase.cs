using PlayerRoles;
using SwiftNPCs;
using SwiftNPCs.Core.Management;
using SwiftNPCs.Core.World.AIModules;
using SwiftTD.Interfaces;
using System;
using UnityEngine;

namespace SwiftTD.Core
{
    [Serializable]
    public class EnemyBase(string id, string displayName) : ISaveAsJSON
    {
        public static readonly string FilePath = Plugin.PluginFolder + "\\Enemies\\";

        public string ID = id;
        public string DisplayName = displayName;

        public int Lives = 1;

        public float Speed;
        public float Health;
        public float Value;

        public RoleTypeId Role;

        public virtual AIPlayerProfile Spawn(SwiftNPCs.Core.Pathing.Path p, Game g)
        {
            if (p.TryGetWaypoint(0, out Vector3 waypoint))
            {
                AIPlayerProfile prof = Utilities.CreatePathAI(Role, waypoint, p);
                prof.WorldPlayer.MovementEngine.SpeedOverride = Speed;
                prof.Player.Health = Health;
                EnemyMarker m = prof.ReferenceHub.gameObject.AddComponent<EnemyMarker>();

                if (prof.WorldPlayer.ModuleRunner.TryGetModule(out AIFollowPath fp))
                    m.FollowPath = fp;
                else
                {
                    prof.Delete();
                    return null;
                }

                m.Enemy = this;
                m.Path = p;
                m.Belonging = g;

                return prof;
            }

            return null;
        }

        public string GetFilePath() => FilePath;

        public string GetID() => ID;
    }
}
