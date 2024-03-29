using SwiftTD.Interfaces;
using System.IO;
using Utf8Json;

namespace SwiftTD
{
    public static class JSONTools
    {
        public static void SaveAsJSON<T>(this T obj) where T : ISaveAsJSON
        {
            obj.SaveAsFile(Path.Combine(obj.GetFilePath(), obj.GetID() + ".json"));
        }

        public static void SaveAsFile<T>(this T obj, string path) where T : IJsonSerializable
        {
            File.WriteAllBytes(path, JSONSerialize(obj));
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
            return JSONConvert<T>(Path.Combine(path, id + ".json"));
        }

        public static bool TryGetSavedFromID<T>(string path, string id, out T output) where T : IJsonSerializable
        {
            output = GetSavedFromID<T>(path, id);
            return output != null;
        }
    }
}
