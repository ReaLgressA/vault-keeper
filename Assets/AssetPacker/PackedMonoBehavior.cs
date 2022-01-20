using System;
using Newtonsoft.Json;
using UnityEngine;

namespace AssetPacker {
    [Serializable]
    public class PackedMonoBehavior {
        [JsonProperty] 
        private string type;

        public PackedMonoBehavior() {}
        
        public PackedMonoBehavior(string type) {
            this.type = type;
        }

        public virtual void Unpack(GameObject go) {
            
        }
    }
}