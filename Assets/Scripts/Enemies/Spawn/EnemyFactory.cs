using Incision.Enemies.Components;
using Incision.Enemies.Templates;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Incision.Enemies.Runtime
{
    public class EnemyFactory
    {
        private static uint created = 0;
        [Inject] private IObjectResolver resolver;

        public Enemy Create(EnemyTemplate definition, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            created++;
            Debug.Log($"<color=orange>[Enemy Factory]</color> Creating <color=yellow>{definition.Id}</color> at {position}");
            var enemy = resolver.Instantiate(definition.Prefab, position, rotation, parent);
            enemy.Initialize(definition);
            enemy.name = $"{definition.Id}_{created}";
            return enemy;
        }
    }
}