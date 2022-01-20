using Newtonsoft.Json;
using UnityEngine;

namespace AssetPacker {
    public static class AssetPacker {
        private static JsonSerializerSettings settings;

        private static JsonSerializerSettings Settings => settings ??= new JsonSerializerSettings {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };
        
        public static string Pack(GameObject go) {
            PackedGameObject packed = PackedGameObject.Pack(go);
            return JsonConvert.SerializeObject(packed, Settings);;
        }

        public static GameObject Unpack(string json, Transform trRoot) {
            PackedGameObject packed = JsonConvert.DeserializeObject<PackedGameObject>(json);
            return PackedGameObject.Unpack(packed, trRoot);
        }
    }
}