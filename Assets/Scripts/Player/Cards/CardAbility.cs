using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        protected CardInstance Owner { get; }

        protected CardAbilityRuntime(CardInstance owner) => Owner = owner;

        // Ќажали умение (после успешной траты боезапаса)
        public virtual void Activate(AbilityContext ctx) { }

        //  арта бросила снар€д: умение может его изменить
        public virtual void OnProjectileSpawned(Projectile projectile, AbilityContext ctx) { }
    }
}
