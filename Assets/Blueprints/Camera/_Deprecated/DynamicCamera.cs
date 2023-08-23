using System.Collections;
using System.Collections.Generic;
using Blueprints.Components;
using Edd.Cameras;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Features.Camera.Deprecated
{
    public class DynamicCamera : MonoComponent
    {
        [field: SerializeField] public List<ICameraRig> AllRigs { get; private set; }
        public UnityEngine.Camera Camera { get; private set; }
        public ICameraRig ActiveRig { get; set; }
        private List<ICameraRig> InactiveRigs { get; set; }
        private ICameraRig Next
            => InactiveRigs[0];
        private ICameraRig Previous
            => InactiveRigs[^1];
        
        public void Init(params ICameraRig[] rigs)
        {
            if(Camera == null)
                Camera = gameObject.ForceComponent<UnityEngine.Camera>();
            
            if (AllRigs.Count == 0)
            {
                Debug.LogError($"{nameof(DynamicCamera)}: requires {nameof(ICameraRig)}'s to function" +
                               $"\r\nCould not initialize {nameof(DynamicCamera)}");
                return;
            }
            
            ActiveRig = AllRigs?[0];
            InactiveRigs = new List<ICameraRig>();
        }

        public void DeInit()
            => Destroy(Camera);

        public void NextRig()
            => ChangeRig(Next);

        public void PreviousRig()
            => ChangeRig(Previous);
        
        private void ChangeRig(ICameraRig rig)
        {
            if (!InactiveRigs.Contains(rig))
            {
                Debug.LogError($"{nameof(DynamicCamera)}: Attempting to change to {nameof(ICameraRig)} that is " +
                               $"not observed by this {nameof(DynamicCamera)}");    
            }
            
            ActiveRig.Deactivate();
            var previousRig = ActiveRig;
            ActiveRig = rig;
            InactiveRigs.Remove(ActiveRig);
            InactiveRigs.Add(previousRig);
            ActiveRig.Activate(this);
        }
        
        private IEnumerator FollowRig()
        {
            var cachedTransform = transform;
            
            while (true)
            {
                cachedTransform.position 
                    = Vector3.MoveTowards(cachedTransform.position, ActiveRig.GhostCamera.position, 1f);
                
                cachedTransform.rotation 
                    = Quaternion.RotateTowards(cachedTransform.rotation, ActiveRig.GhostCamera.rotation, 45);
                yield return null;
            }
        }

        public void OnCameraSwitch(InputAction.CallbackContext _)
            => NextRig();
        
        public void OnScroll(InputAction.CallbackContext context)
            => ActiveRig.OnScroll(context);
        
        public void OnLook(InputAction.CallbackContext context)
            => ActiveRig.OnLook(context);
    }
}