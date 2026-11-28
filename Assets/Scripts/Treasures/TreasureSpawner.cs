using System.Collections.Generic;
using Incision.Extensions;
using Incision.Interfaces;
using Incision.Structs.Events;
using Incision.Treasures.Events;
using Incision.Room.Runtime;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace Incision
{
    public class TreasureSpawner : MonoBehaviour, ITaggable
    {
        public string LOG_TAG { get; } = "[Treasure Spawner]".AsTag(Color.magenta);
        
        [SerializeField] private List<Spawnpoint> _spawnPoints;

        private ObjectFactory _factory;
        private IEventBus _eventBus;
        private RoomNode _roomNode;

        private bool _spawned;
        private bool _subscribed;

        [Inject]
        private void Initialize(ObjectFactory factory, IEventBus eventBus, RoomNode roomNode)
        {
            _factory = factory;
            _eventBus = eventBus;
            _roomNode = roomNode;
            Subscribe();
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            if (!_subscribed)
                return;

            _eventBus.Unsubscribe<SpawnTreasure>(SpawnTreasure);
            _subscribed = false;
        }

        private void Subscribe()
        {
            if (_subscribed || _eventBus == null || !isActiveAndEnabled)
                return;

            _eventBus.Subscribe<SpawnTreasure>(SpawnTreasure);
            _subscribed = true;
        }

        private void SpawnTreasure(SpawnTreasure evt)
        {
            if (_spawned || evt.Node != _roomNode)
                return;

            var treasureTemplate = evt.Node.GetRandomTreasure();
            if (_spawnPoints == null || _spawnPoints.Count == 0)
            {
                _eventBus.Publish(new LogWarning(LOG_TAG, "Treasure spawn points are not assigned."));
                return;
            }

            if (treasureTemplate == null || treasureTemplate.TreasurePrefab == null)
            {
                _eventBus.Publish(new LogWarning(LOG_TAG, "Treasure prefab is not assigned."));
                return;
            }

            var sp = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
            if (sp == null)
                return;

            var treasure = _factory.CreateTreasure(treasureTemplate, sp.transform.position, sp.transform.rotation);
            treasure.transform.SetParent(transform, true);
            _spawned = true;
            
        }
    }
}
