// using System.Collections.Generic;
// using Blueprints.StateMachine.Finite.Core;
// using UnityEngine;
// using UnityEngine.InputSystem;
// using Visitor.DefaultExample;
//
// namespace Player
// {
//     public enum Direction 
//     {
//         Left = -1,
//         Right = 1,
//     }
//
//     public enum PlayerState : byte
//     {
//         Stop,
//         Start,
//         Turn,
//         Crash,
//     }
//
//     public static class PlayerStates
//     {
//         public static PlayerStopState StopState = new(PlayerState.Stop);
//         public static PlayerStartState StartState = new(PlayerState.Start);
//         public static PlayerTurnState LeftTurnState = new(PlayerState.Turn, Direction.Left);
//         public static PlayerTurnState RightTurnState = new(PlayerState.Turn, Direction.Right);
//     }
//     
//     public class PlayerController : FiniteStateMachine<PlayerController>, IVisitable
//     {
//         public float maxSpeed = 2.0f;
//         public float turnDistance = 2.0f;
//         
//         public float CurrentSpeed { get; set; }
//         public Direction CurrentDirection { get; set; }
//
//         public InputActionReference input;
//
//         private readonly IList<IVisitable> _visitables = new List<IVisitable>(); 
//         
//         public PlayerWeapon weapon = new();
//         public PlayerEngine engine = new();
//         public PlayerShield shield = new();
//
//         private void Awake()
//         {
//             CurrentState = PlayerStates.StopState;
//             _visitables.Add(weapon);
//             _visitables.Add(engine);
//             _visitables.Add(shield);
//         }
//
//         public override void HandleInput(InputAction.CallbackContext context)
//         {
//             base.HandleInput(context);
//             // Logging.GameLog.PlayerLogger.Log($"Player Current State {CurrentState}");
//         }
//
//         private void OnEnable()
//         {
//             input.action.started += HandleInput;
//             input.action.performed += HandleInput;
//             input.action.canceled += HandleInput;
//         }
//
//         public void Accept(IVisit visitor)
//         {
//             foreach (var visitable in _visitables)
//             {
//                 visitable.Accept(visitor);
//             }
//         }
//     }
//
//     public class PlayerStopState : FiniteState<PlayerController, PlayerState>
//     {
//         public PlayerStopState(PlayerState state) : base(state) { }
//
//         public override IFiniteState<PlayerController> InputHandler(InputAction.CallbackContext context)
//         {
//             return context.ReadValue<Vector2>() != Vector2.zero ? PlayerStates.StartState : PlayerStates.StopState;
//         }
//
//         public override void Enter(PlayerController component)
//         {
//             component.CurrentSpeed = 0f;
//         }
//     }
//
//     public class PlayerStartState : FiniteState<PlayerController, PlayerState>
//     {
//         public PlayerStartState(PlayerState state) : base(state) { }
//
//         public override IFiniteState<PlayerController> InputHandler(InputAction.CallbackContext context)
//         {
//             var direction = context.ReadValue<Vector2>();
//
//             IFiniteState<PlayerController> state = direction.x switch
//             {
//                 > 0 => PlayerStates.RightTurnState,
//                 < 0 => PlayerStates.LeftTurnState,
//                 _ => PlayerStates.StartState
//             };
//             
//             Debug.Log(context.ReadValue<Vector2>());
//             
//             if (direction == Vector2.zero)
//             {
//                 state = PlayerStates.StopState;
//             }
//
//             return state;
//         }
//
//         public override void Enter(PlayerController component)
//         {
//             component.CurrentSpeed = component.maxSpeed;
//         }
//
//         public override void Update(PlayerController component)
//         {
//             if (component.CurrentSpeed > 0)
//             {
//                 component.transform.Translate(Vector3.forward * (component.CurrentSpeed * Time.deltaTime));
//             }
//         }
//     }
//
//     public class PlayerTurnState : FiniteState<PlayerController, PlayerState>
//     {
//         public PlayerTurnState(PlayerState state, Direction turnDirection) : base(state)
//         {
//             _direction = turnDirection;
//         }
//
//         private readonly Direction _direction;
//         
//         public override IFiniteState<PlayerController> InputHandler(InputAction.CallbackContext context)
//         {
//             var movement = context.ReadValue<Vector2>();
//
//             if (movement == Vector2.zero)
//             {
//                 return PlayerStates.StopState;
//             }
//
//             return PlayerStates.StartState;
//         }
//
//         public override void Enter(PlayerController component)
//         {
//             component.CurrentDirection = _direction;
//             var turnDirection = (float)_direction;
//
//             if (component.CurrentSpeed > 0)
//             {
//                 component.transform.Translate(new Vector3(turnDirection,0,0) * component.turnDistance);
//             }
//         }
//     }
// }