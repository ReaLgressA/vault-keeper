using System.Linq;
using UnityEditor;
using UnityEngine;
using VaultKeeper.Data.PackageContent;

namespace VaultKeeper.Editor {
    public class TextsListEditorView {
        private int selectedEntryIndex = -1;

        private GUIStyle styleLabelCentered = null;
        private GUIStyle styleLabelRichText = null;

        private Vector2 previewScrollPosition;
        
        private void InitializeStyles() {
            styleLabelCentered ??= new GUIStyle(EditorStyles.label) {
                alignment = TextAnchor.MiddleCenter
            };
            styleLabelRichText ??= new GUIStyle(EditorStyles.label) {
                richText = true
            };
        }
        
        public void DrawOnGUI(VaultPackageContentTexts content, Rect windowRect) {
            EditorGUILayout.LabelField(string.Empty, GUI.skin.horizontalSlider);
            GUILayout.Label("TEXT ASSETS", styleLabelCentered);
            InitializeStyles();
            if (Event.current.type == EventType.DragUpdated)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                Event.current.Use();
            }
            if (Event.current.type == EventType.DragPerform) {
                //DragAndDrop.AcceptDrag();
                for (int i = 0; i < DragAndDrop.objectReferences.Length; i++) {
                    Object obj = DragAndDrop.objectReferences[i];
                    Debug.Log(obj.GetType().Name);
                    if (obj is TextAsset textAsset) {
                        content.Texts.Add(new VaultPackageContentTexts.TextAssetSettings(textAsset));
                    } else if (obj is Texture2D texture) {
                        string texturePath = AssetDatabase.GetAssetPath(texture);
                        Object[] objects = AssetDatabase.LoadAllAssetsAtPath(texturePath);
                        TextAsset[] textAssets = objects.Where(q => q is TextAsset).Cast<TextAsset>().ToArray();
                        for (int j = 0; j < textAssets.Length; ++j) {
                            content.Texts.Add( new VaultPackageContentTexts.TextAssetSettings(textAssets[j]));   
                        }
                    }
                }
                //Event.current.Use();
            }
            
            GUIContent[] textEntries = new GUIContent[content.Texts.Count];
            for (int i = 0; i < content.Texts.Count; ++i) {
                GUIContent entryGUIContent = new GUIContent(content.Texts[i].id);
                textEntries[i] = entryGUIContent;
            }
            int itemsPerRow = Mathf.FloorToInt(windowRect.width / 2 / 64f - 1);
            
            GUILayout.BeginHorizontal();            
            selectedEntryIndex = 
                GUILayout.SelectionGrid(selectedEntryIndex, textEntries, itemsPerRow, 
                                        GUILayout.Width(windowRect.width / 2 - 64f),
                                        GUILayout.Height(Mathf.Max(1, content.Texts.Count / itemsPerRow) * 64f));
            GUILayout.Space(20);
            if (HasSelectedTextAsset(content)) {
                VaultPackageContentTexts.TextAssetSettings selectedTextAsset = content.Texts[selectedEntryIndex];
                if (selectedTextAsset != null) {
                    GUILayout.BeginVertical(GUILayout.Width(windowRect.width / 4));
                    previewScrollPosition = GUILayout.BeginScrollView(previewScrollPosition, GUILayout.Width(windowRect.width / 4),
                                                                      GUILayout.MaxHeight(windowRect.height / 3));
                    GUILayout.Label(selectedTextAsset.textAsset?.text ?? "<color=red><b>TEXT ASSET IS MISSING!!!</b></color>", styleLabelRichText);
                    GUILayout.EndScrollView();
                    
                    selectedTextAsset.id = GUILayout.TextField(selectedTextAsset.id, GUILayout.Width(windowRect.width / 4));

                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (HasSelectedTextAsset(content) && GUILayout.Button("Remove")) {
                        content.Texts.RemoveAt(selectedEntryIndex);
                    }
            
                    GUILayout.Space(25);
                    GUILayout.EndHorizontal();
            
                    GUILayout.EndVertical();
                }
            } else {
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
            
            EditorGUILayout.LabelField(string.Empty, GUI.skin.horizontalSlider);
        }

        public bool HasSelectedTextAsset(VaultPackageContentTexts content) {
            if (content?.Texts == null || content.Texts.Count == 0) {
                return false;
            }
            bool hasSelected = selectedEntryIndex >= 0 && selectedEntryIndex < content.Texts.Count;
            if (hasSelected) {
                if (content.Texts[selectedEntryIndex] != null) {
                    return true;
                }
                content.Texts.RemoveAt(selectedEntryIndex);
                return false;
            }
            return false;
        }

        public void Reset() {
            selectedEntryIndex = -1;
        }
    }
}