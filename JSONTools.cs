using SwiftTD.Interfaces;
using System.IO;
using Utf8Json;

namespace SwiftTD
{
    public static class JSONTools
    {
        public static void SaveAsJSON<T>(this T obj) where T : ISaveAsJSON
        {
            File.WriteAllBytes(obj.GetFilePath() + obj.GetID() + ".json", JSONSerialize(obj));
        }

        public static byte[] JSONSerialize<T>(this T obj) where T : IJsonSerializable
        {
            return JsonSerializer.Serialize(obj);
        }

        public static T JSONConvert<T>(string path) where T : IJsonSerializable
        {
            return JsonSerializer.Deserialize<T>(File.ReadAllText(path));
        }

        public static bool TryJSONConvert<T>(string path, out T output) where T : IJsonSerializable
        {
            output = JSONConvert<T>(path);
            return output != null;
        }

        public static T GetSavedFromID<T>(string path, string id) where T : IJsonSerializable
        {
            return JSONConvert<T>(path + id + ".json");
        }

        public static bool TryGetSavedFromID<T>(string path, string id, out T output) where T : IJsonSerializable
        {
            output = GetSavedFromID<T>(path, id);
            return output != null;
        }
    }
}
