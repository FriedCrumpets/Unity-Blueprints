// using Logging;
// using Visitor.DefaultExample;
//
// namespace BluepPlayer
// {
//     public class PlayerAbilities
//     {
//         
//     }
//     
//     [System.Serializable]
//     public class PlayerWeapon : IVisitable
//     {
//         public int range = 5;
//         public int maxRange = 25;
//
//         public float strength = 25f;
//         public float maxStrength = 50f;
//
//         public void Fire()
//         {
//             // GameLog.PlayerLogger.Log("Weapon Fired");
//         }
//
//         public void Accept(IVisit visitor)
//         {
//             visitor.Visit(this);
//         }
//     }
//
//     [System.Serializable]
//     public class PlayerEngine : IVisitable
//     {
//         public float turboBoost = 25;
//         public float maxTurboBoost = 200f;
//
//         private bool _isTurboOn;
//         private float _defaultSpeed = 300f;
//
//         public float CurrentSpeed => _isTurboOn ? _defaultSpeed + turboBoost : _defaultSpeed;
//
//         public void ToggleTurbo() => _isTurboOn = !_isTurboOn;
//
//         public void Accept(IVisit visitor)
//         {
//             visitor.Visit(this);
//         }
//     }
//     
//     [System.Serializable]
//     public class PlayerShield  : IVisitable
//     {
//         public float health = 50f;
//
//         public float Damage(float damage)
//         {
//             health -= damage;
//
//             return health;
//         }
//
//         public void Accept(IVisit visitor)
//         {
//             visitor.Visit(this);
//         }
//     }
// }