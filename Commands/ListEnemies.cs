using CommandSystem;
using SwiftAPI.Commands;
using SwiftTD.Core;

namespace SwiftTD.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ListEnemies : CommandBase
    {
        public override string[] GetAliases() => ["tdlenemies", "tdlmobs"];

        public override string GetCommandName() => "tdlistenemy";

        public override string GetDescription() => "Lists all enemies for tower defense.";

        public override PlayerPermissions[] GetPerms() => null;

        public override bool Function(string[] args, ICommandSender sender, out string result)
        {
            result = "Enemies: \n";

            foreach (EnemyBase enemy in SavesManager.RegisteredEnemies.Values)
                result += "\n" + enemy.GetID() + " | " + enemy.DisplayName;

            return true;
        }
    }
}
