using CommandSystem;
using SwiftAPI.Commands;
using SwiftTD.Core;

namespace SwiftTD.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class CreateWave : CommandBase
    {
        public override string[] GetAliases() => ["tdwave", "tdw"];

        public override string GetCommandName() => "tdcreatewave";

        public override string GetDescription() => "Creates and saves an wave for tower defense.";

        public override PlayerPermissions[] GetPerms() => [PlayerPermissions.PlayersManagement];

        public override bool Function(string[] args, ICommandSender sender, out string result)
        {
            if (!TryGetArgument(args, 1, out string id))
            {
                result = "Please provide an ID! ";

                return false;
            }

            if (!TryGetArgument(args, 2, out string name))
            {
                result = "Please provide a name! ";

                return false;
            }

            SavesManager.SaveWave(new(id, name, []));

            result = "Created wave with ID " + id;

            return true;
        }
    }
}
