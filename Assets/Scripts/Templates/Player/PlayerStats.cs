using Incision.Templates;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatData", menuName = "Scriptable Objects/PlayerStatsData")]
public class PlayerStats : StatsTemplate
{
    [Header("Slam")]
    [SerializeField] private float _slamOverlapRadius = 1f;
    [SerializeField] private float _slamStunTime = 1f;

    public float SlamOverlapRadius => _slamOverlapRadius;
    public float SlamStunTime => _slamStunTime;
}
