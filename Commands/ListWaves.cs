using CommandSystem;
using SwiftAPI.Commands;
using SwiftTD.Core;

namespace SwiftTD.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ListWaves : CommandBase
    {
        public override string[] GetAliases() => ["tdlwaves", "tdlw"];

        public override string GetCommandName() => "tdlistwaves";

        public override string GetDescription() => "Lists all waves for tower defense.";

        public override PlayerPermissions[] GetPerms() => null;

        public override bool Function(string[] args, ICommandSender sender, out string result)
        {
            result = "Waves: \n";

            foreach (Wave wave in SavesManager.RegisteredWaves.Values)
                result += "\n" + wave.GetID() + " | " + wave.DisplayName;

            return true;
        }
    }
}
