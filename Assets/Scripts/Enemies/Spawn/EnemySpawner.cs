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

        private async void Start()
        {
            await StartSpawning();
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
                
                foreach (var entry in wave.Entries)
                {
                    var allowedList = _spawnPoints.Where(sp => sp.IsAllowed(entry.Enemy.Type)).ToArray();
                    var listSize = allowedList.Length;
                    if (listSize == 0)
                    {
                        _eventBus.Publish(new LogError(LOG_TAG, $"Failed to spawn {entry.Enemy.Id.AsTag(Color.red)}. There is no spawn points for them to spawn."));
                        continue;
                    }
                    
                    for (var i = 0; i < entry.Count; i++)
                    {
                        _eventBus.Publish(new Log(LOG_TAG, $"Spawning {$"{entry.Enemy.Id}_{i}".AsTag(Color.red)}"));
                        var point = allowedList[Random.Range(0, listSize)];
                        var enemy = _factory.Create(entry.Enemy, point.transform.position, point.transform.rotation, transform);
                        await Task.Delay(entry.Interval);
                    }
                }
            }
        }
    }
}