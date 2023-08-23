using Blueprints.Components;

namespace Edd.Cameras
{
    public class DynamicCamera : MonoComponent
    {
        // Camera Rig
        /*
         * There are 2 primary camera rigs from the In-Game Player. 
         *  - Shoulder ( combat ) 
         *  - Orbit ( explore )
         * Other Rigs may be activated situationally depending on an event in-scene.
         *
         * There is only one camera throughout the entire experience
         */
        
        protected override void Awake()
        {
            base.Awake();
        }
    }
}