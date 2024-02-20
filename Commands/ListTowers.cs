using CommandSystem;
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
    public class ListTowers : CommandBase
    {
        public override string[] GetAliases() => ["tdltower", "tdlt"];

        public override string GetCommandName() => "tdlisttowers";

        public override string GetDescription() => "Lists all towers for tower defense.";

        public override PlayerPermissions[] GetPerms() => null;

        public override bool Function(string[] args, ICommandSender sender, out string result)
        {
            result = "Towers: \n";

            foreach (TowerBase tower in SavesManager.RegisteredTowers.Values)
                result += "\n" + tower.GetID() + " | " + tower.DisplayName;

            return true;
        }
    }
}
