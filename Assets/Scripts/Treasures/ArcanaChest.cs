using System.Collections;
using Incision.Arcana;
using UnityEngine;

namespace Incision.Treasures
{
    public sealed class ArcanaChest : Treasure
    {
        [SerializeField, Min(0.1f)] private float _openDistance = 2.5f;

        protected override IEnumerator LifeRoutine()
        {
            while (ResolvePlayerTarget(null, _openDistance) == null)
                yield return null;

            Collect();
        }

        protected override void OnPickedUp()
        {
            _eventBus.Publish(new OpenArcanaReward());
        }
    }
}
