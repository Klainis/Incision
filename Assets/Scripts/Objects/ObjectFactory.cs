using Incision.Extensions;
using Incision.Interfaces;
using Incision.Structs.Events;
using Incision.Treasures;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Incision
{
    public class ObjectFactory : ITaggable
    {
        public string LOG_TAG { get; } = "[Object Factory]".AsTag(Color.magenta);

        private static uint _healOrbCreated = 0;

        [Inject] private IObjectResolver _resolver;
        [Inject] private IEventBus _eventBus;

        public HealOrb CreateHealOrb(HealOrbTemplate data, HealOrb prefab, Vector3 position, Quaternion rotation)
        {
            var orb = Create(
                prefab,
                data.ID,
                _healOrbCreated++,
                position, rotation);
            orb.Initialize();
            return orb;
        }

        public Treasure CreateTreasure(BaseTreasureTemplate data, Vector3 position, Quaternion rotation)
        {
            var treasure = Create(data.TreasurePrefab, data.ID, 0, position, rotation);
            treasure.Initialize(_eventBus, data);
            return treasure;
        }

        private T Create<T>(T prefab, string id, uint index, Vector3 position, Quaternion rotation) where T : Component
        {
            T obj = _resolver.Instantiate(prefab, position, rotation);
            obj.name = $"{id}_{index}";

            _eventBus.Publish(
                new Log(
                    LOG_TAG,
                    $"Creating {obj.name.AsTag(Color.yellow)} at {position.ToString().AsTag(Color.yellow)}"
                )
            );

            return obj;
        }
    }
}
