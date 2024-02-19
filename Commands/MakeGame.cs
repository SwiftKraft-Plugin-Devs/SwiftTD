using CommandSystem;
using PlayerRoles;
using SwiftAPI.Commands;
using SwiftNPCs.Core.Pathing;
using SwiftTD.Core;

namespace SwiftTD.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class MakeGame : CommandBase
    {
        public override string[] GetAliases() => ["td"];

        public override string GetCommandName() => "towerdefense";

        public override string GetDescription() => "Starts a game of tower defense on a path.";

        public override PlayerPermissions[] GetPerms() => [PlayerPermissions.RoundEvents];

        public override bool Function(string[] args, ICommandSender sender, out string result)
        {
            if (!TryGetArgument(args, 1, out string arg1) || !int.TryParse(arg1, out int id) || !PathManager.TryGetPath(id, out Path p))
            {
                result = "Please provide a valid path ID! ";

                return false;
            }

            Wave wave1 = JSONTools.GetSavedFromID<Wave>(Wave.FilePath, "WAVE1");
            Wave wave2 = JSONTools.GetSavedFromID<Wave>(Wave.FilePath, "WAVE2");
            Wave wave3 = JSONTools.GetSavedFromID<Wave>(Wave.FilePath, "WAVE3");
            Wave wave4 = JSONTools.GetSavedFromID<Wave>(Wave.FilePath, "WAVE4");
            Wave wave5 = JSONTools.GetSavedFromID<Wave>(Wave.FilePath, "WAVE5");
            Wave wave6 = JSONTools.GetSavedFromID<Wave>(Wave.FilePath, "WAVE6");
            Wave wave7 = JSONTools.GetSavedFromID<Wave>(Wave.FilePath, "WAVE7");
            Wave wave8 = JSONTools.GetSavedFromID<Wave>(Wave.FilePath, "WAVE8");
            Wave wave9 = JSONTools.GetSavedFromID<Wave>(Wave.FilePath, "WAVE9");
            Wave wave10 = JSONTools.GetSavedFromID<Wave>(Wave.FilePath, "WAVE10");
            Game game = new(wave1, wave2, wave3, wave4, wave5, wave6, wave7, wave8, wave9, wave10);

            game.SpawnWaves(p);

            result = "Started tower defense game! ";

            return true;
        }
    }
}
