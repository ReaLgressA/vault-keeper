using System;
using Newtonsoft.Json;
using UnityEngine;

namespace AssetPacker.SerializationExtensions
{
    public class Vector4JsonConverter : JsonConverter<Vector4>
    {
        public override void WriteJson(JsonWriter writer, Vector4 value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("x");
            writer.WriteValue(value.x);
            writer.WritePropertyName("y");
            writer.WriteValue(value.y);
            writer.WritePropertyName("z");
            writer.WriteValue(value.z);
            writer.WritePropertyName("w");
            writer.WriteValue(value.w);
            writer.WriteEndObject();       
        }

        public override Vector4 ReadJson(JsonReader reader,
                                         Type objectType,
                                         Vector4 existingValue,
                                         bool hasExistingValue,
                                         JsonSerializer serializer)
        {
            var deserializedVector = serializer.Deserialize(reader);
            return JsonConvert.DeserializeObject<Vector4>(deserializedVector.ToString());
        }
    }
}