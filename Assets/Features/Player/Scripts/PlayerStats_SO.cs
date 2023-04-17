using Features.Player;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/Player/Data/Stats", order = 0)]
public class PlayerStats_SO : ScriptableObject
{
    [SerializeField] private PlayerStats stats;

    public PlayerStats Stats => stats;
}
