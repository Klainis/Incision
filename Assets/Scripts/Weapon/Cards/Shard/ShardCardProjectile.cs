using System.Collections.Generic;
using Incision;
using Incision.Interfaces;
using UnityEngine;
using VContainer;

public readonly struct ShardEmpower
{
    public readonly int ExtraShards;
    public readonly float OtherCardShare;
    public readonly float OtherCardDamageMultiplier;
    public readonly IReadOnlyList<CardInstance> OtherCards;

    public ShardEmpower(int extraShards, float otherCardShare, float otherCardDamageMultiplier,
        IReadOnlyList<CardInstance> otherCards)
    {
        ExtraShards = extraShards;
        OtherCardShare = otherCardShare;
        OtherCardDamageMultiplier = otherCardDamageMultiplier;
        OtherCards = otherCards;
    }
}

public class ShardCardProjectile : CardProjectile
{
    private ShardProjectile _shardPrefab;
    private int _shardCount = 5;
    private float _angleBetweenShards = 35f;
    private float _maxFanAngle = 360f;

    private float _shardDamageMultiplier = 0.35f;
    private float _shardSpeed = 40f;
    private float _spawnOffset = 0.25f;
    private ForceMode _forceMode = ForceMode.Impulse;

    [Inject] private ProjectileFactory _projectileFactory;

    private ShardEmpower? _empower;
    private bool _hasSplit;

    public void Empower(in ShardEmpower empower) => _empower = empower;

    protected override void OnInitialized()
    {
        ShardCardData shardCardData = CardData as ShardCardData;

        if (shardCardData != null)
        {
            Debug.Log("SPAWN SHARD CART");
            _shardPrefab = shardCardData.ShardPrefab;
            _shardCount = shardCardData.ShardCount;
            _angleBetweenShards = shardCardData.AngleBetweenShards;
            _maxFanAngle += shardCardData.MaxFanAngle;
            _shardDamageMultiplier = shardCardData.ShardDamageMultiplier;
            _shardSpeed = shardCardData.ShardSpeed;
            _spawnOffset = shardCardData.SpawnOffset;
            _forceMode = shardCardData.ForceMode;
        }
    }

    protected override void OnTargetCollision(Collision collision, IDamageable damageable)
    {
        SpawnEffectOnDestroy();
        damageable.TakeDamage(new(Damage, gameObject, collision.gameObject, IsCrit));

        Split(collision, hitTarget: true);
        DisposeProjectile();
    }

    protected override void OnOtherCollision(Collision collision)
    {
        base.OnOtherCollision(collision);
        Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void Split(Collision collision, bool hitTarget)
    {
        if (_hasSplit || _shardPrefab == null || _projectileFactory == null)
            return;
        _hasSplit = true;

        ContactPoint contact = collision.GetContact(0);
        Vector3 normal = contact.normal.normalized;
        Vector3 origin = contact.point + (-normal) * _spawnOffset;

        Vector3 finalDir = (-collision.relativeVelocity).normalized;
        finalDir.y = 0;
        Quaternion axisRotation = Quaternion.LookRotation(finalDir);

        int total = _shardCount + (_empower.HasValue ? _empower.Value.ExtraShards : 0);
        float shardDamage = Damage * _shardDamageMultiplier;

        var shards = new List<CardProjectile>(total);

        for (int i = 0; i < total; i++)
        {
            Vector3 dir = CardDirection(axisRotation, i, total);
            CardProjectile shard = SpawnShard(origin, dir, shardDamage);
            if (shard != null)
                shards.Add(shard);
        }

        IgnoreCollisions(shards, hitTarget ? collision.collider : null);
    }

    private CardProjectile SpawnShard(Vector3 origin, Vector3 dir, float ownShardDamage)
    {
        CardProjectile prefab = _shardPrefab;
        CardData data = CardData;
        float damage = ownShardDamage;
        bool isCrit = IsCrit;
        float speed = _shardSpeed;

        if (TryPickOtherCard(out CardInstance other))
        {
            prefab = other.Data.CardPrefab;
            data = other.Data;
            damage = other.RollDamage(out isCrit) * _empower.Value.OtherCardDamageMultiplier;
            speed = other.Data.ProjectileSpeed;
        }

        CardProjectile shard = _projectileFactory.CreateCardProjectile(
            data, damage, isCrit, origin, Quaternion.LookRotation(dir), prefab);

        shard.Rigidbody.AddForce(dir * speed, _forceMode);
        return shard;
    }

    private bool TryPickOtherCard(out CardInstance card)
    {
        card = null;

        if (!_empower.HasValue) return false;
        ShardEmpower e = _empower.Value;

        if (e.OtherCards == null || e.OtherCards.Count == 0) return false;
        if (Random.value > e.OtherCardShare) return false;

        CardInstance candidate = e.OtherCards[Random.Range(0, e.OtherCards.Count)];
        if (candidate?.Data == null) return false;

        CardProjectile prefab = candidate.Data.CardPrefab;

        if (prefab == null) return false;

        card = candidate;
        return true;
    }

    private Vector3 CardDirection(Quaternion axisRotation, int index, int total)
    {
        float step = _angleBetweenShards;

        if (_maxFanAngle > 0f && total > 1)
            step = Mathf.Min(step, _maxFanAngle / (total - 1));

        float angle = (index - (total - 1) * 0.5f) * step;

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);

        return (axisRotation * rotation * Vector3.forward).normalized;
    }

    private static void IgnoreCollisions(List<CardProjectile> shards, Collider hitCollider)
    {
        var colliders = new List<Collider[]>(shards.Count);
        foreach (CardProjectile s in shards)
            colliders.Add(s.GetComponentsInChildren<Collider>());

        for (int i = 0; i < colliders.Count; i++)
        {
            if (hitCollider != null)
                foreach (Collider c in colliders[i])
                    Physics.IgnoreCollision(c, hitCollider);

            for (int j = i + 1; j < colliders.Count; j++)
                foreach (Collider a in colliders[i])
                    foreach (Collider b in colliders[j])
                        Physics.IgnoreCollision(a, b);
        }
    }
}