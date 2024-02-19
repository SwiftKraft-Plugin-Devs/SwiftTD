using SwiftTD.Interfaces;
using System.IO;

namespace SwiftTD
{
    public static class JSONTools
    {
        public static void SaveAsJSON<T>(this T obj) where T : ISaveAsJSON
        {
            File.WriteAllText(obj.GetFilePath() + obj.GetID() + ".json", JSONSerialize(obj));
        }

        public static string JSONSerialize<T>(this T obj) where T : ISaveAsJSON
        {
            return JsonSerialize.ToJson(obj);
        }

        public static T JSONConvert<T>(string path) where T : ISaveAsJSON
        {
            return JsonSerialize.FromFile<T>(path);
        }

        public static bool TryJSONConvert<T>(string path, out T output) where T : ISaveAsJSON
        {
            output = JSONConvert<T>(path);
            return output != null;
        }

        public static T GetSavedFromID<T>(string path, string id) where T : ISaveAsJSON
        {
            return JSONConvert<T>(path + id + ".json");
        }

        public static bool TryGetSavedFromID<T>(string path, string id, out T output) where T : ISaveAsJSON
        {
            output = GetSavedFromID<T>(path, id);
            return output != null;
        }
    }
}
