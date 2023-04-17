using System.Collections.Generic;
using UnityEngine;

namespace Features.Player
{
    [System.Serializable]
    public class PlayerStats
    {
        [SerializeField] private List<PlayerStat> stats;
        
        public List<PlayerStat> Stats => stats;
        
        // [SerializeField] private PlayerStat health; 
        // [SerializeField] private PlayerStat mana;
        // [SerializeField] private PlayerStat sanity; 
        //
        // public PlayerStat Health => health;
        // public PlayerStat Mana => mana;
        // public PlayerStat Sanity => sanity;
    }
}