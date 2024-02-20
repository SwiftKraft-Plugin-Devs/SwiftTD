using CommandSystem;
using PlayerRoles;
using SwiftAPI.Commands;
using SwiftTD.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftTD.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class CreateTower : CommandBase
    {
        public override string[] GetAliases() => ["tdtower", "tdtw"];

        public override string GetCommandName() => "tdcreatetower";

        public override string GetDescription() => "Creates and saves an tower for tower defense.";

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

            name = name.Replace('_', ' ');

            if (!TryGetArgument(args, 3, out string arg1) || !Enum.TryParse(arg1, out RoleTypeId role))
            {
                result = "Please provide a role! ";

                return false;
            }

            if (!TryGetArgument(args, 4, out string arg2) || !Enum.TryParse(arg2, out ItemType item))
            {
                result = "Please provide the weapon! ";

                return false;
            }

            if (!TryGetArgument(args, 5, out string arg3) || !float.TryParse(arg3, out float cost))
            {
                result = "Please provide the cost! ";

                return false;
            }

            if (!TryGetArgument(args, 6, out string arg4) || !float.TryParse(arg4, out float range))
            {
                result = "Please provide the range! ";

                return false;
            }

            if (!TryGetArgument(args, 7, out string arg5) || !float.TryParse(arg5, out float turn))
            {
                result = "Please provide the turn speed! ";

                return false;
            }

            if (!TryGetArgument(args, 8, out string arg6) || !bool.TryParse(arg6, out bool head))
            {
                result = "Please provide if the tower shoots heads! (true/false) ";

                return false;
            }

            SavesManager.SaveTower(new(id, name, role, item, cost, range, turn, head));

            result = "Created tower! ";

            return true;
        }
    }
}
