using Incision.Interfaces;
using Incision.Portal.Structs.Events;
using UnityEngine;
using VContainer;

namespace Incision.Portal
{
    public class Portal : MonoBehaviour
    {
        [Inject] private IEventBus _eventBus;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) 
                return;
            _eventBus.Publish(new OnPortalEntered());
        }
    }
}
