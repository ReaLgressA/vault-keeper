using System;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

namespace AssetPacker.ComponentPackers {
    [Serializable]
    public class PackedTransform {
        [JsonProperty] 
        private float px, py, pz;
        [JsonProperty] 
        private float sx, sy, sz;
        [JsonProperty] 
        private float rx, ry, rz, rw;

        // [SerializeField] 
        // private List<FieldDescription> fieldsDescriptions = new List<FieldDescription>();

        public PackedTransform() {}
        
        public PackedTransform(Transform transform) {
            Vector3 localPosition = transform.localPosition;
            px = localPosition.x;
            py = localPosition.y;
            pz = localPosition.z;
            Vector3 localScale = transform.localScale;
            sx = localScale.x;
            sy = localScale.y;
            sz = localScale.z;
            Quaternion localRotation = transform.localRotation;
            rx = localRotation.x;
            ry = localRotation.y;
            rz = localRotation.z;
            rw = localRotation.w;
            // var type = typeof(Transform);
            // var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);


            // foreach (var field in fields) {
            //     //field.Name
            //     if (field.IsNotSerialized) {
            //         continue;
            //     }
            //
            //     //fieldsDescriptions.Add(new FieldDescription(field, field.GetValue(transform)));
            // }

            //System.Reflection.FieldInfo.
        }

        public void Unpack(GameObject go) {
            go.transform.localPosition = new Vector3(px, py, pz);
            go.transform.localScale = new Vector3(sx, sy, sz);
            go.transform.localRotation = new Quaternion(rx, ry, rz, rw);
        }

        [Serializable]
        private class FieldDescription {
            [SerializeField] public string name;
            [SerializeField] public string type;
            [SerializeField] public string value;
            
            public FieldDescription(FieldInfo fieldInfo, object value) {
                name = fieldInfo.Name;
                this.value = JsonUtility.ToJson(value, true); 
                type = fieldInfo.FieldType.AssemblyQualifiedName;
            }
        }
    }
}