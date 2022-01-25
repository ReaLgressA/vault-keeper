using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace AssetPacker {
    [Serializable]
    public class PackedMonoBehavior {
        [JsonProperty] 
        private string type;
        [JsonProperty]
        private int instanceId;
        [JsonProperty] 
        private List<PackedComponentField> fields = new List<PackedComponentField>();
        
        public static Type[] excludedTypes = {
            typeof(Canvas), typeof(Transform), typeof(RectTransform)
        };
        
        public PackedMonoBehavior() {}
        
        public PackedMonoBehavior(MonoBehaviour monoBehaviour) {
            Type objectType = monoBehaviour.GetType();
            instanceId = monoBehaviour.GetInstanceID();
            type = objectType.AssemblyQualifiedName;
            FieldInfo[] fields = objectType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo field in fields) {
                bool excludeField = false;
                foreach (var excludedType in excludedTypes) {
                    if (field.FieldType == excludedType) {
                        excludeField = true;
                        break;
                    }
                }
                if (excludeField) {
                    continue;   
                }
                this.fields.Add(new PackedComponentField(field, monoBehaviour));
            }
        }

        public void Unpack(GameObject go) {
            Type objectType = Type.GetType(type);
            Component component = go.AddComponent(objectType);
            foreach (PackedComponentField field in fields) {
                field.SetValue(objectType, field, component);
            }
        }
        
        [Serializable]
        private class PackedComponentField {
            [JsonProperty]
            public string name = null;
            [JsonProperty]
            public string type = null;
            [JsonProperty]
            public string value = null;
            [JsonProperty]
            public InstanceIdReference instanceId = null;
            
            [Preserve]
            public PackedComponentField() {}
             
            public PackedComponentField(FieldInfo fieldInfo, object instance) {
                name = fieldInfo.Name;
                type = fieldInfo.FieldType.AssemblyQualifiedName;
                
                if (fieldInfo.FieldType.IsAssignableFrom(typeof(UnityEngine.Object))
                    || fieldInfo.FieldType.IsSubclassOf(typeof(UnityEngine.Object))) {
                    UnityEngine.Object objectValue = fieldInfo.GetValue(instance) as UnityEngine.Object;
                    instanceId = objectValue == null ? null : new InstanceIdReference(objectValue.GetInstanceID());
                } else {
                    object objectValue = fieldInfo.GetValue(instance);
                    value = objectValue == null ? null : JsonConvert.SerializeObject(objectValue, AssetPacker.Settings);
                }
            }

            public void SetValue(Type objectType, PackedComponentField field, object component) {
                var fieldInfo = objectType.GetField(field.name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (instanceId != null) {
                    Debug.LogError($"INSTANCE ID REFERENCE: '{instanceId.id}'");
                    return;
                }
                //TODO: FontData and such raw data serialization (Font/Material and other bullshit as links to real assets)
                if (value != null) {
                    if (type.Contains("UnityEngine.Material")) {
                        Debug.LogError("HERE");
                    }
                    Debug.Log($"Value: '{value}'");
                    object fieldValue = JsonConvert.DeserializeObject(value, Type.GetType(type), AssetPacker.Settings);
                    fieldInfo.SetValue(component, fieldValue);
                }
            }

            [Serializable]
            public class InstanceIdReference {
                [JsonProperty]
                public int id;

                public InstanceIdReference(int id) {
                    this.id = id;
                }
            }
        }
    }
}