using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BloodParticleCollision : MonoBehaviour
{
    [SerializeField] private float _minDecalSize = 8f;
    [SerializeField] private float _maxDecalSize = 14f;

    private ParticleSystem _particleSystem;
    private readonly List<ParticleCollisionEvent> _events = new List<ParticleCollisionEvent>();

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        int count = _particleSystem.GetCollisionEvents(other, _events);

        for (int i = 0; i < count; i++)
        {
            float size = Random.Range(_minDecalSize, _maxDecalSize);
            DecalBuffer.Instance.SpawnDecal(_events[i].intersection, _events[i].normal, size);
        }
    }
}