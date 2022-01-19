using UnityEditor;
using UnityEngine;
using VaultKeeper.Data;

namespace VaultKeeper.Editor {
    public class VaultPackageEditorView {
        
        private readonly SpriteListEditorView spriteListEditorView = new SpriteListEditorView();
        private readonly TextsListEditorView textsListEditorView = new TextsListEditorView();
        private Vector2 scrollPosition;

        public void DrawOnGUI(VaultPackage package, Rect windowRect) {
            GUILayout.BeginVertical();
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            package.Name = EditorGUILayout.TextField("Name", package.Name);
            package.Label = EditorGUILayout.TextField("Label", package.Label);
            spriteListEditorView.DrawOnGUI(package.ContentSprites, windowRect);
            textsListEditorView.DrawOnGUI(package.ContentTexts, windowRect);
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        public void Reset() {
            spriteListEditorView.Reset();
        }
    }
}