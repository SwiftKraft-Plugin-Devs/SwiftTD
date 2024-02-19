using CommandSystem;
using SwiftAPI.Commands;
using SwiftNPCs.Core.Pathing;
using SwiftTD.Core;
using System.Collections.Generic;
using System.Linq;

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

            if (!TryGetArgument(args, 2, out string arg2) || !int.TryParse(arg2, out int hp))
            {
                result = "Please provide a life count! ";

                return false;
            }

            if (!TryGetArgument(args, 3, out string arg3) || !int.TryParse(arg3, out int wait))
            {
                result = "Please provide a intermission time! ";

                return false;
            }

            List<string> waves = [.. args];

            waves.RemoveRange(0, 4);

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

            Game game = new([.. ws])
            {
                Lives = hp,
                Intermission = wait
            };

            game.SpawnWaves(p);

            result = "Started tower defense game! ";

            return true;
        }
    }
}
