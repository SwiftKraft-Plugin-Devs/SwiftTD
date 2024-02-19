using CommandSystem;
using SwiftAPI.Commands;
using SwiftTD.Core;

namespace SwiftTD.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ClearSegments : CommandBase
    {
        public override string[] GetAliases() => ["tdcseg", "tdcs"];

        public override string GetCommandName() => "tdclearsegments";

        public override string GetDescription() => "Clears all segments for a tower defense wave.";

        public override PlayerPermissions[] GetPerms() => [PlayerPermissions.PlayersManagement];

        public override bool Function(string[] args, ICommandSender sender, out string result)
        {
            if (!TryGetArgument(args, 1, out string id) || !SavesManager.TryGetWave(id, out Wave wave))
            {
                result = "Please provide a valid wave ID! ";

                return false;
            }

            wave.ClearSegments();
            wave.SaveWave();

            result = "Cleared all segments for wave " + wave.GetID();

            return true;
        }
    }
}
