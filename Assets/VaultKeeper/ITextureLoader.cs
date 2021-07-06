using UnityEngine;

namespace VaultKeeper {
    public interface ITextureLoader {
        Texture2D LoadTexture(byte[] bytes);
    }
}