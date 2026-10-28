using Incision;
using UnityEngine;

[CreateAssetMenu(fileName = "DoubleRicochetAbility", menuName = "Scriptable Objects/Abilities/Double Ricochet")]
public class DoubleRicochetAbility : CardAbility
{
    [SerializeField] private float _duration = 5f;
    [SerializeField] private int _bounces = 2;

    public float Duration => _duration;

    public override CardAbilityRuntime CreateRuntime(CardInstance owner) => new Runtime(this, owner);

    private class Runtime : CardAbilityRuntime
    {
        private readonly DoubleRicochetAbility _def;
        private float _endTime;
        private bool _internalSpawn;

        public Runtime(DoubleRicochetAbility def, CardInstance owner) : base(owner) => _def = def;

        public bool IsActive => Time.time < _endTime;

        public override void Activate(AbilityContext ctx)
        {
            _endTime = Time.time + _def._duration;
        }

        public override void OnProjectileSpawned(Projectile projectile, AbilityContext ctx)
        {
            if (_internalSpawn || !IsActive) return;
            if (projectile is not RicochetProjectile ricochet) return;

            var data = ricochet.CardData;
            float damage = ricochet.Damage;
            bool isCrit = ricochet.IsCrit;

            ricochet.SetChain(new RicochetChain
            {
                BouncesLeft = _def._bounces,
                FinalBounce = (origin, dir, ignore, projectileFactory) => FireSecondCard(ctx, projectileFactory, origin, dir, ignore)
            });
        }

        private bool FireSecondCard(AbilityContext ctx, ProjectileFactory projectileFactory, Vector3 origin, Vector3 dir, Collider ignore)
        {
            CardInstance second = null;
            foreach (CardInstance c in ctx.Hand.Hand)
            {
                if (c != null && c != Owner)
                {
                    second = c;
                    break;
                }
            }

            if (second == null || second.IsEmpty)
                return false;

            float damage = second.RollDamage(out bool isCrit);

            // Цепочка нужна только как контейнер фабрики и запуска; отскоков у неё нет
            var chain = new RicochetChain { BouncesLeft = 0};
            CardProjectile projectile = chain.Spawn(projectileFactory, second.Data, damage, isCrit, origin, dir);

            foreach (Collider col in projectile.GetComponentsInChildren<Collider>())
                Physics.IgnoreCollision(col, ignore);

            // Вторая карта не запускает свою цепочку рикошетов
            if (projectile is RicochetProjectile r)
                r.SetChain(chain);

            return true;
        }
    }
}