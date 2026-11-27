using System.Collections.Generic;
using Incision.Enemies.Enums;
using UnityEngine;

namespace Incision.Enemies.Components
{
    public class EnemySpawnpoint : Spawnpoint
    {
        [SerializeField] private List<EnemyType> allowedEnemies;

        public bool IsAllowed(EnemyType type) => allowedEnemies != null && allowedEnemies.Contains(type);
    }
}
