using Incision.Interfaces;
using Incision.Portal.Structs.Events;
using Incision.Room.Runtime;
using UnityEngine;
using VContainer;

namespace Incision.Portal
{
    public class Portal : MonoBehaviour
    {
        [Inject] private IEventBus _eventBus;
        
        private PortalLink _link;

        public void Initialize(PortalLink link)
        {
            _link = link;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) 
                return;
            _eventBus.Publish(new OnPortalEntered(_link));
        }
    }
}
