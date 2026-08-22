using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatData", menuName = "Scriptable Objects/PlayerStatsData")]
public class PlayerStats : ScriptableObject
{
    [Header("HP")]
    [SerializeField] private float _maxHP = 100f;

    public float MaxHP => _maxHP;
}
