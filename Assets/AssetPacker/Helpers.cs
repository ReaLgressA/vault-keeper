using UnityEngine;

namespace AssetPacker {
    public static class Helpers {
        public static T AddComponentIfMissing<T>(this GameObject gameObject) where T : Component {
            return gameObject.TryGetComponent(out T component) ? component : gameObject.AddComponent<T>();
        } 
    }
}