using Incision.Enemies.Components;
using Incision.Enemies.Templates;
using Incision.Extensions;
using Incision.Interfaces;
using Incision.Structs.Events;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Incision.Enemies.Runtime
{
    public class EnemyFactory : ITaggable
    {
        public string LOG_TAG { get; } = "[Enemy Factory]".AsTag(Color.magenta);
        
        private static uint created = 0;
        [Inject] private IObjectResolver resolver;
        [Inject] private IEventBus _eventBus;

        public Enemy Create(EnemyTemplate definition, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            created++;
            _eventBus.Publish(
                new Log(
                    LOG_TAG, 
                    $"Creating {definition.Id.AsTag(Color.yellow)} at {position.ToString().AsTag(Color.yellow)}"
                )
            );
            var enemy = resolver.Instantiate(definition.Prefab, position, rotation, parent);
            enemy.Initialize(definition);
            enemy.name = $"{definition.Id}_{created}";
            return enemy;
        }

    }
}