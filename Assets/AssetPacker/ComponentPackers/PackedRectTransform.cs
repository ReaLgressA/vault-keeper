using System;
using Newtonsoft.Json;
using UnityEngine;

namespace AssetPacker.ComponentPackers {
    [Serializable]
    public class PackedRectTransform : IComponentUnpacker {
        [JsonProperty] 
        private float apx, apy, apz;
        [JsonProperty] 
        private float aMaxX, aMaxY, aMinX, aMinY;
        [JsonProperty]
        private float oMaxX, oMaxY, oMinX, oMinY;
        [JsonProperty] 
        private float pivotX, pivotY;
        [JsonProperty] 
        private float sizeX, sizeY;
        [JsonProperty] 
        private float sx, sy, sz;
        [JsonProperty] 
        private float rx, ry, rz, rw;

        public PackedRectTransform() {}
        
        public PackedRectTransform(RectTransform rectTransform) {
            Vector3 localPosition = rectTransform.anchoredPosition3D;
            apx = localPosition.x;
            apy = localPosition.y;
            apz = localPosition.z;
            Vector3 localScale = rectTransform.localScale;
            sx = localScale.x;
            sy = localScale.y;
            sz = localScale.z;
            Quaternion localRotation = rectTransform.localRotation;
            rx = localRotation.x;
            ry = localRotation.y;
            rz = localRotation.z;
            rw = localRotation.w;
            aMaxX = rectTransform.anchorMax.x;
            aMaxY = rectTransform.anchorMax.y;
            aMinX = rectTransform.anchorMin.x;
            aMinY = rectTransform.anchorMin.y;
            oMaxX = rectTransform.offsetMax.x;
            oMaxY = rectTransform.offsetMax.y;
            oMinX = rectTransform.offsetMin.x;
            oMinY = rectTransform.offsetMin.y;
            pivotX = rectTransform.pivot.x;
            pivotY = rectTransform.pivot.y;
            sizeX = rectTransform.sizeDelta.x;
            sizeY = rectTransform.sizeDelta.y;
        }

        public void Unpack(GameObject go) {
            RectTransform rectTransform = go.AddComponentIfMissing<RectTransform>();
            rectTransform.anchoredPosition3D = new Vector3(apx, apy, apz);
            rectTransform.localScale = new Vector3(sx, sy, sz);
            rectTransform.localRotation = new Quaternion(rx, ry, rz, rw);
            rectTransform.anchorMax = new Vector2(aMaxX, aMaxY);
            rectTransform.anchorMin = new Vector2(aMinX, aMinY);
            rectTransform.offsetMax = new Vector2(oMaxX, oMaxY);
            rectTransform.offsetMin = new Vector2(oMinX, oMinY);
            rectTransform.pivot = new Vector2(pivotX, pivotY);
            rectTransform.sizeDelta = new Vector2(sizeX, sizeY);
        }
    }
}