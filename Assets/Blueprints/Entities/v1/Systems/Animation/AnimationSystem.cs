using System;
using Blueprints.Entities;
using Features.AnimationSys;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Playables;
using UnityEngine.ResourceManagement.AsyncOperations;
using Utils;

namespace Features.Systems
{
    public class AnimationSystem : MonoBehaviour, ISystem<Actor>
    {
        private IDisposable _courierSubscription;
        private AsyncOperationHandle<Avatar> _avatarHandle;
        private AsyncOperationHandle<RuntimeAnimatorController> _controllerHandle;

        void ISystem<Actor>.Init(Actor entity)
        {
            _courierSubscription = entity.Courier.Register(this);
            
            Animator = gameObject.ForceComponent<Animator>();
            Events = gameObject.ForceComponent<AnimationEvents>();
            
            _avatarHandle = Addressables.LoadAssetAsync<Avatar>($"Avatar: {entity.Data.Type.ToString()}");
            _avatarHandle.Completed += handle => Animator.avatar = handle.Result;

            _controllerHandle = Addressables.LoadAssetAsync<RuntimeAnimatorController>($"AnimatorController: {entity.Data.Type}");
            _controllerHandle.Completed += handle => Controller = handle.Result;
            
            FireEvents = true;
            RootMotion = true;
        }
        
        public Animator Animator { get; private set; }
        public AnimationEvents Events { get; private set; }
        public PlayableGraph Graph => Animator.playableGraph;
        
        public RuntimeAnimatorController Controller
        {
            get => Animator.runtimeAnimatorController;
            set => Animator.runtimeAnimatorController = value;
        }
        
        public bool FireEvents
        {
            get => Animator.fireEvents;
            set => Animator.fireEvents = value;
        }

        public bool RootMotion
        {
            get => Animator.hasRootMotion;
            set => Animator.applyRootMotion = value;
        }
        
        void ISystem<Actor>.Deinit()
        {
            _courierSubscription.Dispose();
            Addressables.Release(_avatarHandle.Result);
            Addressables.Release(_controllerHandle.Result);
            Destroy(Animator);
            Destroy(Events);
        }
    }
}