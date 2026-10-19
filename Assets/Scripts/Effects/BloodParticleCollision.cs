using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class BloodParticleCollision : MonoBehaviour
{
    [SerializeField] private float _minDecalSize = 8f;
    [SerializeField] private float _maxDecalSize = 14f;

    [SerializeField] private ParticleSystem _particleSystem;
    private readonly List<ParticleCollisionEvent> _events = new List<ParticleCollisionEvent>();
    
    [Inject] private DecalBuffer _decalBuffer;

    private void OnParticleCollision(GameObject other)
    {
        var count = _particleSystem.GetCollisionEvents(other, _events);
        for (var i = 0; i < count; i++)
        {
            var size = Random.Range(_minDecalSize, _maxDecalSize);
            _decalBuffer.SpawnDecal(_events[i].intersection, _events[i].normal, size);
        }
    }
}