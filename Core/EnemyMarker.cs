using SwiftNPCs.Core.Management;
using SwiftNPCs.Core.Pathing;
using SwiftNPCs.Core.World.AIModules;
using UnityEngine;

namespace SwiftTD.Core
{
    public class EnemyMarker : MonoBehaviour
    {
        public Game Belonging;

        public EnemyBase Enemy;

        public AIFollowPath FollowPath;

        public Path Path;

        bool passed;

        private void Start()
        {
            Belonging?.Enemies.Add(this);
        }

        private void FixedUpdate()
        {
            if (Belonging.Lost)
            {
                passed = true;
                FollowPath.Parent.Core.Profile.Delete();
                return;
            }

            if (passed || FollowPath == null || Belonging == null)
                return;

            if (FollowPath.CurrentIndex >= Path.Waypoints.Count - 1 && GetDistanceToEnd() <= Path.WaypointRadius + 1f)
            {
                passed = true;
                Belonging?.Enemies.Remove(this);
                FollowPath.Parent.Core.Profile.Delete();
                Belonging.EnemyPassed(Enemy.Lives);
            }
        }

        public float GetDistanceToEnd()
        {
            if (Path == null || !Path.TryGetWaypoint(Path.Waypoints.Count - 1, out Vector3 waypoint))
                return Mathf.Infinity;

            return Vector3.Distance(transform.position, waypoint);
        }
    }
}
