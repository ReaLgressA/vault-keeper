using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine;
using VaultKeeper.Utility;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VaultKeeper.Data.PackageContent {
    [Serializable]
    public class VaultPackageContentTexts : VaultPackageContent {
        public override VaultPackageContentType ContentType => VaultPackageContentType.Texts;

        [SerializeField]
        private List<TextAssetSettings> texts = new List<TextAssetSettings>();
            
        public List<TextAssetSettings> Texts => texts;

        [Serializable]
        public class TextAssetSettings {
            [SerializeField] 
            public string id;
            [SerializeField]
            public string text;
            [NonSerialized] 
            public TextAsset textAsset;
            [SerializeField]
            private string assetPath;

            public TextAssetSettings(TextAsset textAsset) {
                this.textAsset = textAsset;
                id = textAsset.name.ToLower();
                text = textAsset.text;
    #if UNITY_EDITOR
                PrepareForSave();
    #endif
            }
            
#if UNITY_EDITOR
            public void PrepareAfterLoading() {
                UnityEngine.Object[] assets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
                for (int i = 0; i < assets.Length; ++i) {
                    if (assets[i] is TextAsset textAsset) {
                        this.textAsset = textAsset;
                        text = textAsset.text;
                        return;
                    }    
                }
                textAsset = null;
            }

            public void PrepareForSave() {
                assetPath = AssetDatabase.GetAssetPath(textAsset);
                text = textAsset.text;
            }
    #endif
            
            public void Export(ZipOutputStream stream, string directoryTextAssets) {
                string path = $"{directoryTextAssets}{id}.txt";
                stream.CreateFileEntry(path, TextSerializer.GetTextBytes(text), CompressionMethod.Stored);
            }
        }

        public void PrepareForSave() {
            
#if UNITY_EDITOR
            foreach (var textAsset in Texts) {
                textAsset.PrepareForSave();
            }
#endif
        }
        
        public void PrepareAfterLoading() {
#if UNITY_EDITOR
            foreach (var textAsset in Texts) {
                textAsset.PrepareAfterLoading();
            }
#endif
        }
        
        public void Export(ZipOutputStream stream, string directoryPackage) {
            string directoryTextAssets = $"{directoryPackage}textAssets/"; 
            stream.CreateDirectoryEntry(directoryTextAssets);
            for (int i = 0; i < texts.Count; ++i) {
                texts[i].Export(stream, directoryTextAssets);    
            }
        }

        public async Task PrepareAfterImport(ZipFile zipFile, string directoryPackage) {
            // string directoryTextAssets = $"{directoryPackage}textAssets/"; 
            // for (int i = 0; i < texts.Count; ++i) {
            //     await texts[i].PrepareAfterImport(zipFile, directoryTextAssets);
            // }
        }
    }
}