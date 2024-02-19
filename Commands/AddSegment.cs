using CommandSystem;
using SwiftAPI.Commands;
using SwiftTD.Core;

namespace SwiftTD.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class AddSegment : CommandBase
    {
        public override string[] GetAliases() => ["tdseg", "tds"];

        public override string GetCommandName() => "tdcreatesegment";

        public override string GetDescription() => "Creates and saves a segment for a tower defense wave.";

        public override PlayerPermissions[] GetPerms() => [PlayerPermissions.PlayersManagement];

        public override bool Function(string[] args, ICommandSender sender, out string result)
        {
            if (!TryGetArgument(args, 1, out string id) || !SavesManager.TryGetWave(id, out Wave wave))
            {
                result = "Please provide a valid wave ID! ";

                return false;
            }

            if (!TryGetArgument(args, 2, out string eid) || !SavesManager.TryGetEnemy(eid, out EnemyBase enemy))
            {
                result = "Please provide a valid enemy ID! ";

                return false;
            }

            if (!TryGetArgument(args, 3, out string arg1) || !int.TryParse(arg1, out int count))
            {
                result = "Please provide a enemy count! ";

                return false;
            }

            if (!TryGetArgument(args, 4, out string arg2) || !float.TryParse(arg2, out float delay))
            {
                result = "Please provide a spawn delay! ";

                return false;
            }

            wave.AddSegment(new(enemy, count, delay));
            wave.SaveWave();

            result = "Added new segment for wave! ";

            return true;
        }
    }
}
