// using System;
// using System.Collections;
// using Blueprints.StateController.Core;
// using UnityEngine;
// using Utility;
//
// namespace Blueprints.StateMachine.StateController.Behaviours
// {
//     public class CanvasFadeState : MonoBehaviour, IState
//     {
//         [SerializeField] private float fadeDuration;
//         [SerializeField] private CanvasGroup canvasGroup;
//
//         [field: SerializeField, Header("State Settings")] public float EnterTime { get; set; }
//         [field: SerializeField] public float IdleTime { get; set; }
//         [field: SerializeField] public float ExitTime { get; set; }
//         
//         public IEnumerator Enter()
//         {
//             yield return Fader.Fade(0, 1, fadeDuration,
//                 () =>
//                 {
//                     canvasGroup.alpha = 0;
//                     canvasGroup.gameObject.SetActive(true);
//                 },
//                 value =>
//                 {
//                     canvasGroup.alpha = value;
//                 });
//         }
//
//         public IEnumerator Idle()
//         {
//             yield return null;
//         }
//
//         public IEnumerator Exit()
//         {
//             yield return Fader.Fade(1, 0, fadeDuration,
//                 () => canvasGroup.gameObject.SetActive(true),
//                 value =>
//                 {
//                     canvasGroup.alpha = value;
//                 }, 
//                 () => canvasGroup.gameObject.SetActive(false));
//         }
//     }
// }