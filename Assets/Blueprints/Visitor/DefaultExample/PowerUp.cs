// using Player;
// using UnityEngine;
//
// namespace Visitor.DefaultExample
// {
//     [CreateAssetMenu(fileName = "PowerUp", menuName = "Player/PowerUp", order = 0)]
//     public class PowerUp : ScriptableObject, IVisit
//     {
//
//         [field: SerializeField] public string Name { get; set; }
//         [field: SerializeField] public string Description { get; set; }
//         
//         [field: SerializeField] public GameObject Prefab { get; set; }
//
//         [Tooltip("Fully Heal Shield")] public bool healShield;
//
//         [Range(0f, 50f)] [Tooltip("Boost turbo settings up to 50/mph")]
//         public float turboBoost;
//         
//         [Range(0, 25)] [Tooltip("Increase weapon range up to 25 units")]
//         public int weaponRange;
//         
//         [Range(0f, 50f)] [Tooltip("Increase weapon strength up to 50%")]
//         public float weaponStrength;
//         
//         void IVisit.Visit(PlayerShield shield)
//         {
//             if (healShield)
//             {
//                 shield.health = 100.0f;
//             }
//         }
//
//         void IVisit.Visit(PlayerWeapon weapon)
//         {
//             var range = weapon.range += weaponRange;
//
//             if (range >= weapon.maxRange)
//             {
//                 weapon.range = weapon.maxRange;
//             }
//             else
//             {
//                 weapon.range = range;
//             }
//
//             var strength = weapon.strength += Mathf.Round(weapon.strength * weaponStrength / 100);
//             if (strength >= weapon.maxStrength)
//             {
//                 weapon.strength = weapon.maxStrength;
//             }
//             else
//             {
//                 weapon.strength = strength;
//             }
//         }
//
//         void IVisit.Visit(PlayerEngine engine)
//         {
//             var boost = engine.turboBoost += turboBoost;
//
//             if (boost < 0.0f)
//             {
//                 engine.turboBoost = 0f;
//             }
//
//             if (boost >= engine.maxTurboBoost)
//             {
//                 engine.turboBoost = engine.maxTurboBoost;
//             }
//         }
//     }
// }