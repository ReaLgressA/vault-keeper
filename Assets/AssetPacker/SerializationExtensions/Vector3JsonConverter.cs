using System;
using Newtonsoft.Json;
using UnityEngine;

namespace AssetPacker.SerializationExtensions
{
    public class Vector3JsonConverter : JsonConverter<Vector3>
    {
        public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("x");
            writer.WriteValue(value.x);
            writer.WritePropertyName("y");
            writer.WriteValue(value.y);
            writer.WritePropertyName("z");
            writer.WriteValue(value.z);
            writer.WriteEndObject();       
        }

        public override Vector3 ReadJson(JsonReader reader,
                                         Type objectType,
                                         Vector3 existingValue,
                                         bool hasExistingValue,
                                         JsonSerializer serializer)
        {
            var deserializedVector = serializer.Deserialize(reader);
            return JsonConvert.DeserializeObject<Vector3>(deserializedVector.ToString());
        }
    }
}