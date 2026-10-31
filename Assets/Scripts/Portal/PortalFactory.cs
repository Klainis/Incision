using Incision.Extensions;
using Incision.Interfaces;
using Incision.Structs.Events;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Incision.Portal
{
    public class PortalFactory : ITaggable
    {
        public string LOG_TAG { get; } = "[Portal Factory]".AsTag(Color.magenta);

        private int created = 0;
        [Inject] private IObjectResolver resolver;
        [Inject] private IEventBus _eventBus;
        
        public Portal Create(Portal definition, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            created++;
            _eventBus.Publish(
                new Log(
                    LOG_TAG, 
                    $"Creating {definition.name.AsTag(Color.yellow)} at {position.ToString().AsTag(Color.yellow)}"
                )
            );
            var p = resolver.Instantiate(definition, position, rotation, parent);
            p.name = $"{definition.name}_{created}";
            return p;
        }
    }
}
