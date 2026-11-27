using Incision.Templates;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatData", menuName = "Scriptable Objects/PlayerStatsData")]
public class PlayerStats : StatsTemplate
{
    [Header("Slam")]
    [SerializeField] private float _slamOverlapRadius = 1f;
    [SerializeField] private List<StatusEffectTemplate> _statusEffects;

    public float SlamOverlapRadius => _slamOverlapRadius;
    public List<StatusEffectTemplate> StatusEffects => _statusEffects;
}
