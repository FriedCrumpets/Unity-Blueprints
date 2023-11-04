using UnityEngine;

namespace Blueprints.Utility
{
    public static class LayerExt
    {
        public static void RemoveLayerFromCamera(this Camera camera, int layer)
            => camera.cullingMask &= ~( 1 << layer );

        public static void AddLayerToCamera(this Camera camera, int layer)
            => camera.cullingMask |= ~( 1 << layer );

        public static void SetLayer(this GameObject obj, int layer)
            => obj.layer = layer;
    }
}