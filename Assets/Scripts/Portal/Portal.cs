using Incision.Interfaces;
using Incision.Managers;
using Incision.Portal.Structs.Events;
using Incision.Room.Runtime;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Incision.Portal
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private Image _portalIcon;

        [Inject] private IEventBus _eventBus;
        
        private PortalLink _link;

        public void Initialize(PortalLink link)
        {
            _link = link;
            _portalIcon.sprite = link.Icon;
            _portalIcon.enabled = link.Icon != null;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) 
                return;
            _eventBus.Publish(new OnPortalEntered(_link));
        }
    }
}
