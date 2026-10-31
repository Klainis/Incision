using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Incision.Enemies.Runtime;
using Incision.Enemies.Structs.Events;
using Incision.Enemies.Templates;
using Incision.Extensions;
using Incision.Interfaces;
using Incision.Structs.Events;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace Incision.Enemies.Components
{
    public class EnemySpawner : MonoBehaviour, ITaggable
    {
        public string LOG_TAG { get; } = "[Enemy Spawner]".AsTag(Color.magenta);
        
        [SerializeField] private WavePatternTemplate _waveTemplate;
        [SerializeField] private List<Spawnpoint> _spawnPoints;

        [Inject] private EnemyFactory _factory;
        [Inject] private IEventBus _eventBus;
        
        private int enemiesSpawned = 0;
        private List<Enemy> _enemies = new List<Enemy>();

        void Start()
        {
            _eventBus.Subscribe<OnCreatureDied>(OnCreatureDied);
        }

        void OnDestroy()
        {
            _eventBus.Unsubscribe<OnCreatureDied>(OnCreatureDied);
        }

        public async Task StartSpawning()
        {
            await SpawnRoutine();
        }

        private async Task SpawnRoutine()
        {
            foreach (var wave in _waveTemplate.Waves)
            {
                await Task.Delay(wave.StartDelay);
                _eventBus.Publish(new OnWaveStarted(wave));
                await SpawnWave(wave.Entries);
                _eventBus.Publish(new OnWaveEnded(wave));
            }
        }

        private async Task SpawnWave(List<SpawnEntry> entries)
        {
            foreach (var entry in entries)
            {
                var allowedList = _spawnPoints.Where(sp => sp.IsAllowed(entry.Enemy.Type)).ToArray();
                var listSize = allowedList.Length;
                if (listSize == 0)
                {
                    _eventBus.Publish(new LogError(LOG_TAG, $"Failed to spawn {entry.Enemy.Id.AsTag(Color.red)}. There is no spawn points for them to spawn."));
                    continue;
                }
                var point = allowedList[Random.Range(0, listSize)];
                await SpawnEnemy(entry.Enemy, entry.Count, entry.Interval, point);
            }
        }

        private async Task SpawnEnemy(EnemyTemplate tpl, int count, int interval, Spawnpoint point)
        {
            for (var i = 0; i < count; i++)
            {
                _eventBus.Publish(new Log(LOG_TAG, $"Spawning {$"{tpl.Id}_{i}".AsTag(Color.red)}"));
                var enemy = _factory.Create(tpl, point.transform.position, point.transform.rotation, transform);
                _enemies.Add(enemy);
                enemiesSpawned++;
                _eventBus.Publish(new OnEnemyCountChanged(_enemies.Count, enemiesSpawned));
                await Task.Delay(interval);
            }
        }

        private void OnCreatureDied(OnCreatureDied evt)
        {
            if (!evt.Target.TryGetComponent(out Enemy enemy))
                return;

            _enemies.Remove(enemy);
            _eventBus.Publish(new OnEnemyCountChanged(_enemies.Count, enemiesSpawned));
        }
    }
}