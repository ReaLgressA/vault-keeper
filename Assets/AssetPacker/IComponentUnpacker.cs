using UnityEngine;

namespace AssetPacker.ComponentPackers {
    public interface IComponentUnpacker {
        void Unpack(GameObject go);
    }
}