using System.Threading.Tasks;
using Incision.Enemies.Interfaces;
using Incision.Enemies.Runtime;
using Incision.Enemies.Structs.Events;
using Incision.Enemies.Templates;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace Incision.Enemies.Components
{
    public class EnemySpawner : MonoBehaviour
    {
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
                _eventBus.Publish(new WaveStarted(wave));
                // Debug.Log($"Spawning {wave}");
                foreach (var entry in wave.Entries)
                {
                    for (int i = 0; i < entry.Count; i++)
                    {
                        Debug.Log($"Spawning {entry.Enemy.Id}_{i}");
                        var point = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
                        var enemy = _factory.Create(entry.Enemy, point.position, point.rotation, transform);
                        await Task.Delay(entry.Interval);
                    }
                }
            }
        }
    }
}