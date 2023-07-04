using Blueprints.Components;
using Blueprints.XR;

namespace Features.XR
{
    public class XRPlayer : MonoComponent
    {
        protected override void Awake()
        {
            base.Awake();
            
            Add(new XRInput());
        }
    }
}