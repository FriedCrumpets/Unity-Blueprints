using UnityEngine;

namespace Blueprints.Utility.Extensions
{
    public static class RectTransformExt
    {
        public static void SetAnchor(this RectTransform transform, Vector2 anchor)
        {
            var rect = transform.rect;
            var width = rect.width;
            var height = rect.height;
            
            transform.anchorMin = anchor;
            transform.anchorMax = anchor;
            transform.pivot = anchor;

            transform.sizeDelta = new Vector2(width, height);
        }

        public static void SetTopAnchor(this RectTransform transform)
            => transform.SetAnchor(new Vector2(.5f, 1));
        
        public static void SetTopLeftAnchor(this RectTransform transform)
            => transform.SetAnchor(new Vector2(0, 1));
    }
}