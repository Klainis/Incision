using UnityEngine;
using VContainer;

namespace Incision.Portal.Components
{
    public class PortalSpawner : MonoBehaviour
    {
        [SerializeField] private Spawnpoint _spawnpoint;
        [SerializeField] private Portal _portalPrefab;
        
        [Inject] private PortalFactory _portalFactory;

        public void SpawnPortal()
        {
            var p = _portalFactory.Create(_portalPrefab, _spawnpoint.transform.position, _spawnpoint.transform.rotation, gameObject.transform);
        }
    }
}
