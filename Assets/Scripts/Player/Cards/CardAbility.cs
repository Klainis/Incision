using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Incision
{
    public readonly struct AbilityContext
    {
        public readonly CardHand Hand;
        public readonly GunAttack Gun;
        public readonly CardInstance Card;

        public AbilityContext(CardHand hand, GunAttack gun, CardInstance card)
        {
            Hand = hand;
            Gun = gun;
            Card = card;
        }
    }

    public abstract class CardAbility : ScriptableObject
    {
        public abstract CardAbilityRuntime CreateRuntime(CardInstance owner);
    }

    public abstract class CardAbilityRuntime
    {
        [Inject] public ProjectileFactory projectileFactory;

        protected CardInstance Owner { get; }

        protected CardAbilityRuntime(CardInstance owner) => Owner = owner;

        public virtual void Activate(AbilityContext ctx) { }

        public virtual void OnProjectileSpawned(Projectile projectile, AbilityContext ctx) { }
    }
}
