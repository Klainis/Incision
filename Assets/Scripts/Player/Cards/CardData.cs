using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public class CardData : ScriptableObject
{
    [Header("Visual")]
    [SerializeField] private string _cardName;
    [SerializeField] private Sprite _cardIcon;
    [SerializeField] private Material _cardHandMaterial;

    [Header("Card Projectile")]
    [SerializeField] private Projectile _projectilePrefab;

    [Header("Card Stats")]
    [SerializeField] private float _damage = 20f;
    [SerializeField] private int _maxAmmo = 20;
    [SerializeField] private float _attackSpeed = 1.5f;
    [SerializeField][Range(0f, 1f)] private float _critChance = 0.1f;
    [SerializeField] private float _critDamageMultiplier = 1.5f;

    public string CardName => _cardName;
    public Sprite CardIcon => _cardIcon;
    public Material CardHandMaterial => _cardHandMaterial;
    public Projectile CardPrefab => _projectilePrefab;
    public float Damage => _damage;
    public float AttackSpeed => _attackSpeed;
    public int MaxAmmo => _maxAmmo;
    public float CritChance => _critChance;
    public float CritDamageMultiplier => _critDamageMultiplier;
}
