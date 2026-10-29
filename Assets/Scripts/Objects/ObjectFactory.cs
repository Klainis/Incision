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
        public string LOG_TAG { get; } = "[Enemy Factory]".AsTag(Color.magenta);

        private static uint _healOrbCreated = 0;
        [Inject] private IObjectResolver resolver;
        [Inject] private IEventBus _eventBus;

        public Enemy Create(EnemyTemplate definition, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            _healOrbCreated++;
            _eventBus.Publish(
                new Log(
                    LOG_TAG,
                    $"Creating {definition.Id.AsTag(Color.yellow)} at {position.ToString().AsTag(Color.yellow)}"
                )
            );
            var enemy = resolver.Instantiate(definition.Prefab, position, rotation, parent);
            enemy.Initialize(definition);
            enemy.name = $"{definition.Id}_{_healOrbCreated}";
            return enemy;
        }
    }
}
