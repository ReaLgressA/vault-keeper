using System;
using Newtonsoft.Json;
using UnityEngine;

namespace AssetPacker.SerializationExtensions
{
    public class Vector2JsonConverter : JsonConverter<Vector2>
    {
        public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("x");
            writer.WriteValue(value.x);
            writer.WritePropertyName("y");
            writer.WriteValue(value.y);
            writer.WriteEndObject();       
        }

        public override Vector2 ReadJson(JsonReader reader,
                                         Type objectType,
                                         Vector2 existingValue,
                                         bool hasExistingValue,
                                         JsonSerializer serializer)
        {
            var deserializedVector = serializer.Deserialize(reader);
            return JsonConvert.DeserializeObject<Vector2>(deserializedVector.ToString());
        }
    }
}