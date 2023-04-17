using UnityEngine;

namespace Features.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerStats_SO stats;

        public PlayerStats Stats => stats.Stats;
    }
}
