using MetaSpaces.Plaza.Player;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Utils;

namespace Features.XR
{
    public class XRPlayerDeprecated : MonoBehaviour
    {
        [SerializeField] private MovementSettings_SO movementSettings;
        
        private XROrigin _xrOrigin;
        private LocomotionSystem _locomotionSystem;
        private XRPlayerVignette _playerVignette;
        
        public MovementSettings_SO MovementSettings => 
            movementSettings = Utilities.LoadResourceIfNull(movementSettings, "XRMovementSettings");
        [field: SerializeField] public XRRigReferences Rig { get; private set; }
        [field: SerializeField] public XRMovementProvider MovementProvider { get; private set; }

        public XRPlayerVignette playerVignette
        {
            get => _playerVignette ??= new XRPlayerVignette();
            set => _playerVignette = value;
        }
        
        private void OnEnable()
        {
            MovementProvider?.Enable(this);
            playerVignette?.Enable(this);
        }

        private void Awake()
        {
            transform.parent = null;
        }

        private void Start()
        {
            Rig.Initialise();
            MovementProvider.Initialise(this);
            playerVignette.Initialise(this);
        }

        private void OnDisable()
        {
            MovementProvider?.Disable(this);
            playerVignette?.Disable(this);
        }
    }
}