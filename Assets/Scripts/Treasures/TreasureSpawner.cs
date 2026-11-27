using System;
using System.Collections.Generic;
using Incision.Extensions;
using Incision.Interfaces;
using Incision.Structs.Events;
using Incision.Treasures.Enums;
using Incision.Treasures.Events;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace Incision
{
    public class TreasureSpawner : MonoBehaviour, ITaggable
    {
        public string LOG_TAG { get; } = "[Treasure Spawner]".AsTag(Color.magenta);
        
        [SerializeField] private List<Spawnpoint> _spawnPoints;

        [Inject] private ObjectFactory _factory;
        [Inject] private IEventBus _eventBus;

        private uint created = 0;

        private void OnEnable()
        {
            _eventBus.Subscribe<SpawnTreasure>(SpawnTreasure);
        }

        private void OnDisable()
        {
            _eventBus.Unsubscribe<SpawnTreasure>(SpawnTreasure);
        }

        private void SpawnTreasure(SpawnTreasure evt)
        {
            var treasureTemplate = evt.Node.GetRandomTreasure();
            if (_spawnPoints == null || _spawnPoints.Count == 0)
            {
                _eventBus.Publish(new LogWarning(LOG_TAG, "Treasure spawn points are not assigned."));
                return;
            }

            var prefab = treasureTemplate.TreasurePrefab;
            if (prefab == null)
            {
                _eventBus.Publish(new LogWarning(LOG_TAG, "Treasure prefab is not assigned."));
                return;
            }

            if (treasureTemplate != null && treasureTemplate.Type != TreasureType.GoldBag)
                return;

            var sp = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
            if (sp == null)
                return;

            var treasure = _factory.Create(
                prefab,
                prefab.name,
                created++,
                sp.transform.position,
                sp.transform.rotation,
                t => t.Initialize(_eventBus, treasureTemplate));
            treasure.transform.SetParent(transform, true);
            
        }
    }
}
