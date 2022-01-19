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
    public class Vault {
        [SerializeField] private List<VaultPackage> packages = new List<VaultPackage>();

        public List<VaultPackage> Packages => packages;
        
        public string FilePath { get; set; } = null;

        public void AddPackage(VaultPackage package) {
            packages.Add(package);
        }

        public static VaultScriptableObjectWrapper LoadVault(string filePath) {
            VaultScriptableObjectWrapper vaultWrapper = (VaultScriptableObjectWrapper)
                ScriptableObject.CreateInstance(typeof(VaultScriptableObjectWrapper));

            string json = File.ReadAllText(filePath);
            vaultWrapper.vault = JsonUtility.FromJson<Vault>(json);
            vaultWrapper.vault.FilePath = filePath;
            vaultWrapper.vault.PrepareAfterLoading();
            return vaultWrapper;
        }
        
        public void SaveVault(string filePath) {
            PrepareForSave();
            string json = JsonUtility.ToJson(this, true);
            File.WriteAllText(filePath, json);
        }

        public static async Task<VaultScriptableObjectWrapper> ImportWrappedVault(string importFilePath) {
            VaultScriptableObjectWrapper vaultWrapper = (VaultScriptableObjectWrapper)
                ScriptableObject.CreateInstance(typeof(VaultScriptableObjectWrapper));
            
            ZipFile zipFile = new ZipFile(File.OpenRead(importFilePath), false);
            
            string json = await zipFile.LoadText("vault.json");
            vaultWrapper.vault = JsonUtility.FromJson<Vault>(json);
            vaultWrapper.vault.FilePath = null;

            await vaultWrapper.vault.PrepareAfterImport(zipFile);
            
            return vaultWrapper;
        }
        
        public static async Task<Vault> ImportVault(string importFilePath) {
            ZipFile zipFile = new ZipFile(File.OpenRead(importFilePath), false);
            string json = await zipFile.LoadText("vault.json");
            Vault vault = JsonUtility.FromJson<Vault>(json);
            await vault.PrepareAfterImport(zipFile);
            return vault;
        }

        public void ExportVault(string exportFilePath) {
            PrepareForSave();
            string json = JsonUtility.ToJson(this, true);
            string zipPath = Path.Combine(exportFilePath);
            using (ZipOutputStream zipStream = new ZipOutputStream(File.Create(zipPath)))
            {
                zipStream.SetLevel(9);
                zipStream.NameTransform = null;
                zipStream.UseZip64 = UseZip64.Off;

                string directoryPackages = "packages/";
                
                zipStream.CreateDirectoryEntry(directoryPackages);
                
                for (int i = 0; i < Packages.Count; ++i) {
                    Packages[i].Export(zipStream, directoryPackages);
                }
                
                zipStream.CreateFileEntry("vault.json", json);
                zipStream.Close();
            }
        }

        public void GetSprites(string packageLabel, List<VaultPackageContentSprites.SpriteSettings> sprites) {
            for (int i = 0; i < Packages.Count; ++i) {
                Packages[i].GetPackageSprites(packageLabel, sprites);
            }
        }
        
        public void GetTextAssets(string packageLabel, List<VaultPackageContentTexts.TextAssetSettings> textAssets) {
            for (int i = 0; i < Packages.Count; ++i) {
                Packages[i].GetPackageTextAssets(packageLabel, textAssets);
            }
        }
        
        public VaultPackageContentSprites.SpriteSettings GetSprite(string id) {
            for (int i = 0; i < Packages.Count; ++i) {
                var sprite = Packages[i].GetSprite(id);
                if (sprite != null) {
                    return sprite;
                }
            }
            return null;
        }
        
        public VaultPackageContentTexts.TextAssetSettings GetTextAsset(string id) {
            for (int i = 0; i < Packages.Count; ++i) {
                var sprite = Packages[i].GetTextAsset(id);
                if (sprite != null) {
                    return sprite;
                }
            }
            return null;
        }
        
        private async Task PrepareAfterImport(ZipFile zipFile) {
            for (int i = 0; i < Packages.Count; ++i) {
                await Packages[i].PrepareAfterImport(zipFile, "packages/");
            }
        }

        private void PrepareForSave() {
            for (int i = 0; i < Packages.Count; ++i) {
                Packages[i].PrepareForSave();
            }
        }
        
        private void PrepareAfterLoading() {
            for (int i = 0; i < Packages.Count; ++i) {
                Packages[i].PrepareAfterLoading();
            } 
        }
    }
}