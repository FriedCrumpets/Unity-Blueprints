using System;
using Blueprints.Boot;

namespace Features.XR
{
    public class XRSystemBoot : TBoot
    {
        private void Awake() => Boot();

        protected override void CreateSingletons()
        {
            
        }

        private void OnDestroy()
        {
            foreach (var loader in Loaded)
            {
                Destroy(loader);
            }
        }
    }
}
