using System.Collections.Generic;
using Incision.Enemies.Enums;
using UnityEngine;

namespace Incision
{
    public class Spawnpoint : MonoBehaviour
    {
        [SerializeField] private List<EnemyType> allowedEnemies;
        public bool IsAllowed(EnemyType type) => allowedEnemies.Contains(type);

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 2f);
        }
    }
}
