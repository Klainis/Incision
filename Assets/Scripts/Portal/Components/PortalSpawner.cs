using Incision.Room.Components;
using Incision.Room.Runtime;
using UnityEngine;
using VContainer;

namespace Incision.Portal.Components
{
    public class PortalSpawner : MonoBehaviour
    {
        [SerializeField] private Portal _portalPrefab;
        
        [Inject] private PortalFactory _portalFactory;

        public void SpawnPortal(PortalLink link)
        {
            var p = _portalFactory.Create(_portalPrefab, link.Anchor.transform.position, link.Anchor.transform.rotation, gameObject.transform);
            p.Initialize(link);
        }
    }
}
