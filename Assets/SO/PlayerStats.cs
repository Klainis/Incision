using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatData", menuName = "Scriptable Objects/PlayerStatsData")]
public class PlayerStats : ScriptableObject
{
    [Header("HP")]
    [SerializeField] private float _maxHP = 100f;

    [Header("Slam")]
    [SerializeField] private float _slamOverlapRadius = 1f;
    [SerializeField] private float _slamStunTime = 1f;

    public float MaxHP => _maxHP;
    public float SlamOverlapRadius => _slamOverlapRadius;
    public float SlamStunTime => _slamStunTime;
}
