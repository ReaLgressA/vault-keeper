using UnityEngine;

namespace AssetPacker {
    public class AssetPackerComponent : MonoBehaviour {
        [SerializeField]
        private GameObject packTarget;
        [SerializeField, Multiline(10)]
        private string packedJson;

        [SerializeField]
        private GameObject restorationRoot;
        [SerializeField, Multiline(10)]
        private string jsonRestorationSource;
        
        public void Pack() {
            if (packTarget == null) {
                Debug.LogError("Pack target is null!");
                return;
            }
            packedJson = AssetPacker.Pack(packTarget);
            Debug.Log($"Packed to json:\n{packedJson}");
        }

        public void Restore() {
            AssetPacker.Unpack(jsonRestorationSource, restorationRoot?.transform);
        }
    }
}
