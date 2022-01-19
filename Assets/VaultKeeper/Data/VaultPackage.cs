using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine;
using VaultKeeper.Data.PackageContent;
using VaultKeeper.Utility;

namespace VaultKeeper.Data {
    [Serializable]
    public class VaultPackage {
        [HideInInspector, SerializeField] 
        private string name;
        [HideInInspector, SerializeField] 
        private string label;
        [HideInInspector, SerializeField]
        private VaultPackageContentSprites contentSprites = new VaultPackageContentSprites();
        [HideInInspector, SerializeField]
        private VaultPackageContentTexts contentTexts = new VaultPackageContentTexts();
        
        public string Name { get => name; set => name = value; }
        public string Label { get => label; set => label = value; }

        public VaultPackageContentSprites ContentSprites => contentSprites;
        public VaultPackageContentTexts ContentTexts => contentTexts;

        public VaultPackage() { }

        public VaultPackage(string name) {
            this.name = name;
        }

        public void PrepareForSave() {
            contentSprites.PrepareForSave();
            contentTexts.PrepareForSave();
        }

        public void PrepareAfterLoading() {
            contentSprites.PrepareAfterLoading();
            contentTexts.PrepareAfterLoading();
        }
        
        public void SaveContent(string contentRootPath) {
            string packageRootPath = Path.Combine(contentRootPath, name);
            Directory.CreateDirectory(packageRootPath);
            contentSprites.SaveContent(packageRootPath);
        }

        public void Export(ZipOutputStream stream, string directoryRoot) {
            string directoryPackage = $"{directoryRoot}{Name}/"; 
            stream.CreateDirectoryEntry(directoryPackage);
            contentSprites.Export(stream, directoryPackage);
            contentTexts.Export(stream, directoryPackage);
        }
        
        public async Task PrepareAfterImport(ZipFile zipFile, string directoryRoot) {
            string directoryPackage = $"{directoryRoot}{Name}/";
            await contentSprites.PrepareAfterImport(zipFile, directoryPackage);
            await contentTexts.PrepareAfterImport(zipFile, directoryPackage);
        }

        public void GetPackageSprites(string packageLabel, List<VaultPackageContentSprites.SpriteSettings> sprites) {
            if (!string.IsNullOrWhiteSpace(packageLabel) 
                && !Label.Equals(packageLabel, StringComparison.Ordinal)) {
                return;
            }
            for (int i = 0; i < ContentSprites.Sprites.Count; ++i) {
                sprites.Add(ContentSprites.Sprites[i]);
            }
        }

        public void GetPackageTextAssets(string packageLabel, List<VaultPackageContentTexts.TextAssetSettings> texts) {
            if (!string.IsNullOrWhiteSpace(packageLabel) 
                && !Label.Equals(packageLabel, StringComparison.Ordinal)) {
                return;
            }
            for (int i = 0; i < ContentSprites.Sprites.Count; ++i) {
                texts.Add(ContentTexts.Texts[i]);
            }
        }
        
        public VaultPackageContentSprites.SpriteSettings GetSprite(string id) {
            for (int i = 0; i < ContentSprites.Sprites.Count; ++i) {
                if (string.Equals(ContentSprites.Sprites[i].id, id, StringComparison.Ordinal)) {
                    return ContentSprites.Sprites[i];
                }
            }
            return null;
        }

        public VaultPackageContentTexts.TextAssetSettings GetTextAsset(string id) {
            for (int i = 0; i < ContentTexts.Texts.Count; ++i) {
                if (string.Equals(ContentTexts.Texts[i].id, id, StringComparison.Ordinal)) {
                    return ContentTexts.Texts[i];
                }
            }
            return null;
        }
    }
}