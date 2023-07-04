using Blueprints.Utility;

namespace Blueprints.XR
{
    public partial class XRInput : IService { }

    public static class Input
    {
        public static XRInput input;

        static Input()
        {
            input = new XRInput();
            input.Enable();
        }
    }
}