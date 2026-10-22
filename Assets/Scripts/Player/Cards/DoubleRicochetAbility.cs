using Incision;
using UnityEngine;

[CreateAssetMenu(fileName = "DoubleRicochetAbility", menuName = "Scriptable Objects/Abilities/Double Ricochet")]
public class DoubleRicochetAbility : CardAbility
{
    [SerializeField] private float _duration = 5f;
    [SerializeField] private int _bounces = 2;
    [SerializeField] private float _followUpOffset = 0.6f;

    public float Duration => _duration;

    public override CardAbilityRuntime CreateRuntime(CardInstance owner) => new Runtime(this, owner);

    private class Runtime : CardAbilityRuntime
    {
        private readonly DoubleRicochetAbility _def;
        private float _endTime;

        public Runtime(DoubleRicochetAbility def, CardInstance owner) : base(owner) => _def = def;

        public bool IsActive => Time.time < _endTime;

        public override void Activate(AbilityContext ctx)
        {
            _endTime = Time.time + _def._duration;
        }

        public override void OnProjectileSpawned(Projectile projectile, AbilityContext ctx)
        {
            if (!IsActive) return;

            RicochetProjectile ricochet = projectile as RicochetProjectile;
            if (ricochet == null) return;

            ricochet.ConfigureRicochet(
                _def._bounces,
                (origin, dir, ignore) => FireSecondCard(ctx, origin, dir, ignore));
        }

        // Последний рикошет: бросает вторая карта в руке
        private bool FireSecondCard(AbilityContext ctx, Vector3 origin, Vector3 dir, Collider ignore)
        {
            CardInstance second = null;
            foreach (CardInstance c in ctx.Hand.Hand)
            {
                if (c != null && c != Owner) { second = c; break; }
            }

            if (second == null || second.IsEmpty) return false;

            float damage = second.RollDamage(out bool isCrit);

            // applyAbility: false — умение второй карты не должно срабатывать от чужого броска
            Projectile p = ctx.Gun.SpawnProjectile(
                /*second, origin + dir * _def._followUpOffset, dir, damage, isCrit, applyAbility: false*/);

            foreach (Collider col in p.GetComponentsInChildren<Collider>())
                Physics.IgnoreCollision(col, ignore);

            // Вторая карта не запускает свою цепочку рикошетов
            if (p is RicochetProjectile r)
                r.ConfigureRicochet(0, null);

            return true;
        }
    }
}