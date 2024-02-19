using CommandSystem;
using SwiftAPI.Commands;
using SwiftTD.Core;

namespace SwiftTD.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class RemoveSegment : CommandBase
    {
        public override string[] GetAliases() => ["tdreg", "tdrs"];

        public override string GetCommandName() => "tdremovesegment";

        public override string GetDescription() => "Removes a segment from a tower defense wave.";

        public override PlayerPermissions[] GetPerms() => [PlayerPermissions.PlayersManagement];

        public override bool Function(string[] args, ICommandSender sender, out string result)
        {
            if (!TryGetArgument(args, 1, out string wid) || !SavesManager.TryGetWave(wid, out Wave wave))
            {
                result = "Please provide a valid wave ID! ";

                return false;
            }

            if (!TryGetArgument(args, 2, out string arg1) || !int.TryParse(arg1, out int id))
            {
                result = "Please provide a enemy count! ";

                return false;
            }

            wave.RemoveSegment(id);
            wave.SaveWave();

            result = "Removed segment at " + id + " for wave! ";

            return true;
        }
    }
}
