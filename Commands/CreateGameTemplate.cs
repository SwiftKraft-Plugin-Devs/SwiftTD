using CommandSystem;
using SwiftAPI.Commands;
using SwiftTD.Core;
using System.Collections.Generic;

namespace SwiftTD.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class CreateGameTemplate : CommandBase
    {
        public override string[] GetAliases() => ["tdgt", "tdgametemp"];

        public override string GetCommandName() => "tdcreategametemplate";

        public override string GetDescription() => "Creates and saves a game template for tower defense.";

        public override PlayerPermissions[] GetPerms() => [PlayerPermissions.PlayersManagement];

        public override bool Function(string[] args, ICommandSender sender, out string result)
        {
            if (!TryGetArgument(args, 1, out string arg1))
            {
                result = "Please provide an ID! ";

                return false;
            }

            if (!TryGetArgument(args, 2, out string arg2))
            {
                result = "Please provide a display name! ";

                return false;
            }

            List<string> waves = [.. args];

            waves.RemoveRange(0, 3);

            List<Wave> ws = [];

            foreach (string w in waves)
            {
                if (SavesManager.TryGetWave(w, out Wave wav))
                    ws.Add(wav);
            }

            if (ws.Count <= 0)
            {
                result = "Please provide at least 1 valid wave! ";

                return false;
            }

            new GameTemplate(arg1, arg2, ws).RegisterGameTemplate();

            result = "Created new game template!";

            return true;
        }
    }
}
