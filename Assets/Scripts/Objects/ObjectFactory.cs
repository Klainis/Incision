using Incision.Enemies.Components;
using Incision.Enemies.Templates;
using Incision.Extensions;
using Incision.Interfaces;
using Incision.Structs.Events;
using System;
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
            return Create(
                prefab,
                data.ID,
                _healOrbCreated++,
                position, rotation,
                obj => obj.Initialize());
        }

        //TODO: for another type of objects (gold bag, etc.) :)

        private T Create<T>(T prefab, string id, uint index, Vector3 position, Quaternion rotation, Action<T> initialize) where T : Component
        {
            T obj = _resolver.Instantiate(prefab, position, rotation);
            initialize(obj);
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
