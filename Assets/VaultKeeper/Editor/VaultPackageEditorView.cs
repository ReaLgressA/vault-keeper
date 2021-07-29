using UnityEditor;
using UnityEngine;
using VaultKeeper.Data;

namespace VaultKeeper.Editor {
    public class VaultPackageEditorView {
        
        private readonly SpriteListEditorView spriteListEditorView = new SpriteListEditorView();

        public void DrawOnGUI(VaultPackage package, Rect windowRect) {
            GUILayout.BeginVertical();
            package.Name = EditorGUILayout.TextField("Name", package.Name);
            package.Label = EditorGUILayout.TextField("Label", package.Label);
            spriteListEditorView.DrawOnGUI(package.ContentSprites, windowRect);
            GUILayout.EndVertical();
        }

        public void Reset() {
            spriteListEditorView.Reset();
        }
    }
}