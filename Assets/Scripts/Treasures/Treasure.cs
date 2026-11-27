using Incision.Enemies.Interfaces;
using Incision.Interfaces;
using UnityEngine;
using VContainer;

namespace Incision.Treasures
{
    public class Treasure : AttractedPickup
    {
        protected IEventBus _eventBus;
        [Inject] private ITargetProvider _targetProvider;
        private int _amount;
        
        protected BaseTreasureTemplate _template;
        
        public void Initialize(IEventBus eventBus, BaseTreasureTemplate template) {
            _eventBus = eventBus;
            _template = template;
        }

        protected override bool DropsOnSpawn => false;
        protected override float LifeTime => 0f;

        protected override Transform ResolvePlayerTarget(Transform currentTarget, float attractDistance)
        {
            return _eventBus == null ? null : _targetProvider?.GetTarget(transform, attractDistance);
        }
    }
}
