using UnityEditor;
using UnityEngine;

namespace AssetPacker.Editor {
    [CustomEditor(typeof(AssetPackerComponent))]
    public class AssetPackerComponentEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            
            if (GUILayout.Button("Pack")) {
                var assetPackerComponent = target as AssetPackerComponent;
                assetPackerComponent.Pack();
            }
            if (GUILayout.Button("Restore")) {
                var assetPackerComponent = target as AssetPackerComponent;
                assetPackerComponent.Restore();
            }
        }
    }
}