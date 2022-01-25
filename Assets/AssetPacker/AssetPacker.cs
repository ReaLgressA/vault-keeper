using System.Collections.Generic;
using AssetPacker.SerializationExtensions;
using Newtonsoft.Json;
using UnityEngine;

namespace AssetPacker {
    public static class AssetPacker {
        private static JsonSerializerSettings settings;

        public static JsonSerializerSettings Settings => settings ??= new JsonSerializerSettings {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            
            Converters = new List<JsonConverter> {
                new Vector2JsonConverter(),
                new Vector3JsonConverter(),
                new Vector4JsonConverter(),
                new ColorJsonConverter()
            }
        };
        
        public static string Pack(GameObject go) {
            PackedGameObject packed = PackedGameObject.Pack(go);
            return JsonConvert.SerializeObject(packed, Settings);
        }

        public static GameObject Unpack(string json, Transform trRoot) {
            PackedGameObject packed = JsonConvert.DeserializeObject<PackedGameObject>(json);
            return PackedGameObject.Unpack(packed, trRoot);
        }
    }
}