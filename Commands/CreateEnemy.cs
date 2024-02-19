using CommandSystem;
using PlayerRoles;
using SwiftAPI.Commands;
using SwiftTD.Core;
using System;

namespace SwiftTD.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class CreateEnemy : CommandBase
    {
        public override string[] GetAliases() => ["tdenemy", "tdmob"];

        public override string GetCommandName() => "tdcreateenemy";

        public override string GetDescription() => "Creates and saves an enemy for tower defense.";

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

            if (!TryGetArgument(args, 3, out string arg1) || !Enum.TryParse(arg1, out RoleTypeId role))
            {
                result = "Please provide a role! ";

                return false;
            }

            if (!TryGetArgument(args, 4, out string arg2) || !float.TryParse(arg2, out float speed))
            {
                result = "Please provide the speed! ";

                return false;
            }

            if (!TryGetArgument(args, 5, out string arg3) || !float.TryParse(arg3, out float health))
            {
                result = "Please provide the health! ";

                return false;
            }

            if (!TryGetArgument(args, 6, out string arg4) || !float.TryParse(arg4, out float value))
            {
                result = "Please provide the money given when killed! ";

                return false;
            }

            int lives = 1;

            if (TryGetArgument(args, 7, out string arg5) && int.TryParse(arg5, out int l))
                lives = l;

            SavesManager.SaveEnemy(new(id, name) { Health = health, Role = role, Speed = speed, Value = value, Lives = lives });

            result = "Created enemy! ";

            return true;
        }
    }
}
