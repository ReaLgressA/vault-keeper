using System;
using Newtonsoft.Json;
using UnityEngine;

namespace AssetPacker.ComponentPackers {
    [Serializable]
    public class PackedCanvas : IComponentUnpacker {
        [JsonProperty] 
        private AdditionalCanvasShaderChannels additionalShaderChannels;
        [JsonProperty]
        private float normalizedSortingGridSize;
        [JsonProperty]
        private bool overridePixelPerfect;
        [JsonProperty]
        private bool overrideSorting;
        [JsonProperty]
        private bool pixelPerfect;
        [JsonProperty]
        private float planeDistance;
        [JsonProperty]
        private float referencePixelsPerUnit;
        [JsonProperty]
        private RenderMode renderMode;
        [JsonProperty]
        private float scaleFactor;
        [JsonProperty]
        private int sortingLayerID;
        [JsonProperty]
        private string sortingLayerName;
        [JsonProperty]
        private int sortingOrder;
        [JsonProperty]
        private int targetDisplay;

        public static Type ComponentType => typeof(Canvas);
        
        public PackedCanvas() {}
        
        public static bool CanPack(GameObject go) {
            return go.TryGetComponent(ComponentType, out Component component) && component is Canvas;
        }

        public PackedCanvas(Canvas canvas) {
            additionalShaderChannels = canvas.additionalShaderChannels;
            normalizedSortingGridSize = canvas.normalizedSortingGridSize;
            overridePixelPerfect = canvas.overridePixelPerfect;
            overrideSorting = canvas.overrideSorting;
            pixelPerfect = canvas.pixelPerfect;
            planeDistance = canvas.planeDistance;
            referencePixelsPerUnit = canvas.referencePixelsPerUnit;
            renderMode = canvas.renderMode;
            scaleFactor = canvas.scaleFactor;
            sortingLayerID = canvas.sortingLayerID;
            sortingLayerName = canvas.sortingLayerName;
            sortingOrder = canvas.sortingOrder;
            targetDisplay = canvas.targetDisplay;
        }

        public void Unpack(GameObject go) {
            Canvas canvas = go.AddComponentIfMissing<Canvas>();
            canvas.additionalShaderChannels = additionalShaderChannels;
            canvas.normalizedSortingGridSize = normalizedSortingGridSize;
            canvas.overridePixelPerfect = overridePixelPerfect;
            canvas.overrideSorting = overrideSorting;
            canvas.pixelPerfect = pixelPerfect;
            canvas.planeDistance = planeDistance;
            canvas.referencePixelsPerUnit = referencePixelsPerUnit;
            canvas.renderMode = renderMode;
            canvas.scaleFactor = scaleFactor;
            canvas.sortingLayerID = sortingLayerID;
            canvas.sortingLayerName = sortingLayerName;
            canvas.sortingOrder = sortingOrder;
            canvas.targetDisplay = targetDisplay;
        }
    }
}