using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Incision
{
    [CreateAssetMenu(fileName = "ShardsCardData", menuName = "Scriptable Objects/CardData/ShardsCardData")]
    public class ShardCardData : CardData
    {
        [Header("Shards")]
        [SerializeField] private ShardProjectile _shardPrefab;
        [SerializeField, Min(1)] private int _shardCount = 5;
        [SerializeField, Range(5f, 90f)] private float _angleBetweenShards = 35f;
        [SerializeField, Min(0f)] private float _maxFanAngle = 360f;
        [SerializeField, Min(0f)] private float _spawnOffset = 0.25f;

        [Header("Stats")]
        [SerializeField, Range(0f, 1f)] private float _shardDamageMultiplier = 0.35f;
        [SerializeField, Min(0f)] private float _shardSpeed = 40f;

        [Space(5)]
        [SerializeField] private ForceMode _forceMode = ForceMode.Impulse;

        public ShardProjectile ShardPrefab => _shardPrefab;
        public int ShardCount => _shardCount;
        public float AngleBetweenShards => _angleBetweenShards;
        public float MaxFanAngle => _maxFanAngle;
        public float SpawnOffset => _spawnOffset;
        public float ShardDamageMultiplier => _shardDamageMultiplier;
        public float ShardSpeed => _shardSpeed;
        public ForceMode ForceMode => _forceMode;
    }
}
