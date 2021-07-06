using UnityEngine;

namespace Camara {
    public interface ITextureLoader {
        Texture2D LoadTexture(byte[] bytes);
    }
}