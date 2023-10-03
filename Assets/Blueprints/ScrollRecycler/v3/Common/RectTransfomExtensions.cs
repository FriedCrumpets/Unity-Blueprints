using UnityEngine;

namespace Blueprints.Scroller
{
    public static class RectTransfomExtensions
    {
        public static void SetAnchor(this RectTransform rectTransform, Vector2 anchor)
        {
            //Saving to reapply after anchoring. Width and height changes if anchoring is change. 
            var rect = rectTransform.rect;
            var width = rect.width;
            var height = rect.height;
            
            //Setting top anchor 
            rectTransform.anchorMin = anchor;
            rectTransform.anchorMax = anchor;
            rectTransform.pivot = anchor;

            //Reapply size
            rectTransform.sizeDelta = new Vector2(width, height);
        }

        /// <summary>
        /// Anchoring cell and content rect transforms to top preset.
        /// </summary>
        /// <param name="rectTransform"></param>
        public static void SetTopAnchor(this RectTransform rectTransform)
            => SetAnchor(rectTransform, new Vector2(.5f, 1));

        /// <summary>
        /// Anchoring cell and content rect transforms to top-left preset.
        /// </summary>
        /// <param name="rectTransform"></param>
        public static void SetTopLeftAnchor(this RectTransform rectTransform)
            => SetAnchor(rectTransform, new Vector2(0, 1));
        
        public static Vector3[] GetCorners(this RectTransform rectTransform)
        {
            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            return corners;
        }

        public static Vector3 GetBottomLeftCorner(this RectTransform rectTransform)
            => rectTransform.GetCorners()[0];
        
        public static Vector3 GetTopLeftCorner(this RectTransform rectTransform)
            => rectTransform.GetCorners()[1];
        
        public static Vector3 GetTopRightCorner(this RectTransform rectTransform)
            => rectTransform.GetCorners()[2];
        
        public static Vector3 GetBottomRightCorner(this RectTransform rectTransform)
            => rectTransform.GetCorners()[3];
        
    }
}