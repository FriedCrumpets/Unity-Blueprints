using UnityEngine;

namespace Blueprints.Entities.Systems.Camera
{
    public class CameraSystem : MonoBehaviour, ISystem<Actor>
    {
        
        /*
         * Camera systems are different. They need to be handled in a much different manner.
         * Each Ooooo each camera registers to a singleton Camera "Manager" which deals with a Bus... ? no...
         * BUT I do think cameras should be individual to each player
         * Spawned and initialised on each player, but for local players they are activated
         * When a player dies they can view a listing of cameras.
         * Sooo CameraSystems are registered with a CameraManager and can be individually activated
         *
         * Each camera system should have an 
         *  - Over the shoulder camera
         *  - Orbital Camera
         */
        
        public void Init(Actor entity)
        {
            
        }
        
        public void Deinit()
        {
            
        }
    }
}