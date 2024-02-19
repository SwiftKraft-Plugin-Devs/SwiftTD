using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;
using SwiftShops.API;
using SwiftTD.Core;
using System.Collections.Generic;

namespace SwiftTD
{
    public class EventHandler
    {
        [PluginEvent(ServerEventType.PlayerDeath)]
        public void PlayerDeath(PlayerDeathEvent _event)
        {
            if (_event.Player.ReferenceHub.transform.TryGetComponent(out EnemyMarker marker))
            {
                marker.Belonging?.Enemies.Remove(marker);

                List<Player> players = Player.GetPlayers();

                players.RemoveAll((p) => string.IsNullOrEmpty(p.Nickname));

                foreach (Player p in players)
                    p.SetBalance(p.GetBalance() + marker.Enemy.Value);
            }
        }

        [PluginEvent(ServerEventType.CassieAnnouncesScpTermination)]
        public bool CassieAnnouncesScpTermination(CassieAnnouncesScpTerminationEvent _event)
        {
            if (_event.Player.ReferenceHub.transform.TryGetComponent(out EnemyMarker _))
                return false;
            return true;
        }
    }
}
