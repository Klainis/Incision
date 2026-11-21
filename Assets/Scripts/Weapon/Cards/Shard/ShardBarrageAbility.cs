using System.Collections.Generic;
using Incision;
using UnityEngine;

[CreateAssetMenu(fileName = "ShardBarrageAbility", menuName = "Scriptable Objects/Abilities/ShardBarrageAbility")]
public class ShardBarrageAbility : CardAbility
{
    [SerializeField, Min(0.1f)] private float _duration = 6f;
    [SerializeField, Min(0)] private int _extraShards = 5;
    [SerializeField, Range(0f, 1f)] private float _otherCardShare = 0.4f;
    [SerializeField, Min(0f)] private float _otherCardDamageMultiplier = 0.5f;

    public override CardAbilityRuntime CreateRuntime(CardInstance owner)
        => new ShardBarrageRuntime(owner, this);

    private class ShardBarrageRuntime : CardAbilityRuntime
    {
        private readonly ShardBarrageAbility _data;
        private float _activeUntil;

        public ShardBarrageRuntime(CardInstance owner, ShardBarrageAbility data) : base(owner)
        {
            _data = data;
        }

        public override void Activate(AbilityContext ctx)
        {
            _activeUntil = Time.time + _data._duration;
        }

        public override void OnProjectileSpawned(Projectile projectile, AbilityContext ctx)
        {
            if (Time.time > _activeUntil) return;
            if (projectile is not ShardCardProjectile shard) return;

            shard.Empower(new ShardEmpower(
                _data._extraShards,
                _data._otherCardShare,
                _data._otherCardDamageMultiplier,
                CollectOtherCards(ctx)));
        }

        private List<CardInstance> CollectOtherCards(AbilityContext ctx)
        {
            var list = new List<CardInstance>();
            foreach (CardInstance card in ctx.Hand.Hand)
            {
                if (card != null && card != Owner)
                    list.Add(card);
            }
            return list;
        }
    }
}