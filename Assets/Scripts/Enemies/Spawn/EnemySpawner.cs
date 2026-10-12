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
        [SerializeField] private Transform[] _spawnPoints;

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
                // Debug.Log($"Spawning {wave}");
                foreach (var entry in wave.Entries)
                {
                    for (int i = 0; i < entry.Count; i++)
                    {
                        _eventBus.Publish(new Log(LOG_TAG, $"Spawning {entry.Enemy.Id}_{i}"));
                        var point = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
                        var enemy = _factory.Create(entry.Enemy, point.position, point.rotation, transform);
                        await Task.Delay(entry.Interval);
                    }
                }
            }
        }
    }
}