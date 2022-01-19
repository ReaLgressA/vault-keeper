using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine;
using VaultKeeper.Utility;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VaultKeeper.Data.PackageContent {
    [Serializable]
    public class VaultPackageContentSprites : VaultPackageContent {
        public override VaultPackageContentType ContentType => VaultPackageContentType.Sprites;

        [SerializeField] 
        private List<SpriteSettings> sprites = new List<SpriteSettings>();

        public List<SpriteSettings> Sprites => sprites;

        [Serializable]
        public class SpriteSettings {
            [SerializeField] public string id;
            [NonSerialized] public Sprite sprite;
            
            [SerializeField] private string assetPath;
            [SerializeField] private string spriteName;
            [SerializeField] private Vector2 pivot;
            [SerializeField] private Rect rect;
            [SerializeField] private SpriteMeshType spriteMeshType;
            [SerializeField] private float pixelsPerUnit;
            [SerializeField] private uint spriteExtrude;
            [SerializeField] private Vector4 spriteBorder;
            [SerializeField] private bool generateFallbackPhysicsShape;
            [SerializeField] private FilterMode filterMode;
            [SerializeField] private TextureFormat textureFormat;
            [SerializeField] private bool isSRGB;

            public SpriteSettings(Sprite sprite) {
                this.sprite = sprite;
                id = sprite.name.ToLower();
#if UNITY_EDITOR
                PrepareForSave();
#endif
            }

            
#if UNITY_EDITOR
            public void PrepareAfterLoading() {
                Object[] assets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
                for (int i = 0; i < assets.Length; ++i) {
                    if (assets[i] is Sprite spriteAsset && string.Equals(spriteAsset.name, spriteName)) {
                        sprite = spriteAsset;
                        return;
                    }    
                }
                sprite = null;
            }

            public void PrepareForSave() {
                spriteName = sprite.name;
                assetPath = AssetDatabase.GetAssetPath(sprite);
                pivot = sprite.pivot;
                rect = sprite.rect;
                pixelsPerUnit = sprite.pixelsPerUnit;
                spriteBorder = sprite.border;
                filterMode = sprite.texture.filterMode;
                textureFormat = sprite.texture.format;
                
                TextureImporterSettings settings = new TextureImporterSettings();
                TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                importer!.ReadTextureSettings(settings);
                isSRGB = settings.sRGBTexture;
                spriteMeshType = settings.spriteMeshType;
                spriteExtrude = settings.spriteExtrude;
                generateFallbackPhysicsShape = settings.spriteGenerateFallbackPhysicsShape;
            }
#endif

            public void Export(ZipOutputStream stream, string directorySprites) {
                string path = $"{directorySprites}{id}.png";
                stream.CreateFileEntry(path, TextureSerializer.GetSpriteBytes(sprite), CompressionMethod.Stored);
            }
            
            public async Task PrepareAfterImport(ZipFile zipFile, string directorySprites) {
                Texture2D texture = await zipFile.LoadTexture2D($"{directorySprites}{id}.png", textureFormat, isSRGB);
                texture.filterMode = filterMode;
                Rect fixedRect = new Rect(0, 0, Mathf.FloorToInt(rect.width), Mathf.FloorToInt(rect.height));
                
                sprite = Sprite.Create(texture, fixedRect, pivot, pixelsPerUnit, spriteExtrude, spriteMeshType,
                                       spriteBorder, generateFallbackPhysicsShape);
            }
        }

        public void PrepareForSave() {
#if UNITY_EDITOR
            for (int i = 0; i < sprites.Count; ++i) {
                sprites[i].PrepareForSave();
            }
#endif
        }
        
        public void PrepareAfterLoading() {
#if UNITY_EDITOR
            for (int i = 0; i < sprites.Count; ++i) {
                sprites[i].PrepareAfterLoading();
            }
#endif
        }
        
        public async Task PrepareAfterImport(ZipFile zipFile, string directoryPackage) {
            string directorySprites = $"{directoryPackage}sprites/"; 
            for (int i = 0; i < sprites.Count; ++i) {
                await sprites[i].PrepareAfterImport(zipFile, directorySprites);
            }
        }

        public void SaveContent(string contentRootPath) {
            for (int i = 0; i < sprites.Count; ++i) {
                TextureSerializer.SaveSpriteAsTexture(sprites[i].sprite, contentRootPath, sprites[i].id);
            }
        }

        public void Export(ZipOutputStream stream, string directoryPackage) {
            string directorySprites = $"{directoryPackage}sprites/"; 
            stream.CreateDirectoryEntry(directorySprites);
            for (int i = 0; i < sprites.Count; ++i) {
                sprites[i].Export(stream, directorySprites);    
            }
        }
    }
}