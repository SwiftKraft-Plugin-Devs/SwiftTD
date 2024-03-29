using SwiftTD.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace SwiftTD.Core
{
    public class GameTemplate(string id, string displayName, List<Wave> waves) : ISaveAsJSON
    {
        public static string FilePath = Path.Combine(Plugin.PluginFolder, "Game Templates");

        public string ID = id;
        public string DisplayName = displayName;

        public List<Wave> Waves { get; set; } = waves;

        public string GetFilePath() => FilePath;

        public string GetID() => ID;
    }
}
