using UnityEngine;

namespace VaultKeeper {
    public class TextureLoaderPNG : ITextureLoader {
        public Texture2D LoadTexture(byte[] bytes) {
            Texture2D texture2D = new Texture2D(1, 1);
            texture2D.LoadImage(bytes, false);
            return texture2D;
        }
    }
}