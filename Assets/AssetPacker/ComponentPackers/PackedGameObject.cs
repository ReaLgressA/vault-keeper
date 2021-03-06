using System;
using System.Collections.Generic;
using AssetPacker.ComponentPackers;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace AssetPacker {
    [Serializable]
    public class PackedGameObject {
        [JsonProperty]
        private string name;
        [JsonProperty]
        private int instanceId;
        [JsonProperty]
        private PackedTransform transform = null;
        [JsonProperty]
        private PackedRectTransform rectTransform = null;
        [JsonProperty]
        private PackedCanvas canvas = null;
        [JsonProperty]
        private PackedGameObject[] children;
        [JsonProperty]
        private PackedMonoBehavior[] components;

        public static GameObject Unpack(PackedGameObject packed, Transform trRoot) {
            GameObject go = new GameObject(packed.name);
            go.transform.SetParent(trRoot);
            if (packed.rectTransform != null) {
                packed.rectTransform.Unpack(go);
            } else {
                packed.transform.Unpack(go);
            }
            
            packed.canvas?.Unpack(go);
            foreach (PackedMonoBehavior component in packed.components) {
                component.Unpack(go);
            }
            foreach (PackedGameObject child in packed.children) {
                Unpack(child, go.transform);
            }
            return go;
        }
        
        public static PackedGameObject Pack(GameObject gameObject) {
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            return new PackedGameObject {
                name = gameObject.name,
                instanceId = gameObject.GetInstanceID(),
                rectTransform = rectTransform == null ? null : new PackedRectTransform(rectTransform),
                transform = rectTransform == null ? new PackedTransform(gameObject.transform) : null,
                canvas = PackedCanvas.CanPack(gameObject) ? new PackedCanvas(gameObject.GetComponent<Canvas>()) : null,
                components = PackComponents(gameObject),
                children = PackChildren(gameObject)
            };
        }

        private static PackedGameObject[] PackChildren(GameObject root) {
            PackedGameObject[] children = new PackedGameObject[root.transform.childCount];
            for (int i = 0; i < root.transform.childCount; ++i) {
                children[i] = Pack(root.transform.GetChild(i).gameObject);
            }
            return children;
        }

        private static PackedMonoBehavior[] PackComponents(GameObject gameObject) {
            List<PackedMonoBehavior> components = new List<PackedMonoBehavior>();
            var monoBehaviours = gameObject.GetComponents<MonoBehaviour>();
            
            foreach (var c in monoBehaviours) {
                components.Add(new PackedMonoBehavior(c));
            }
            return components.ToArray();
        }
    }
}