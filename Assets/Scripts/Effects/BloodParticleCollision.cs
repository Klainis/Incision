using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BloodParticleCollision : MonoBehaviour
{
    [SerializeField] private DecalProjector _bloodDecalPrefab;
    [SerializeField] private float _minDecalSize = 8f;
    [SerializeField] private float _maxDecalSize = 14f;

    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        int eventCount = _particleSystem.GetSafeCollisionEventSize();

        ParticleCollisionEvent[] events =
            new ParticleCollisionEvent[eventCount];

        int count = _particleSystem.GetCollisionEvents(other, events);

        for (int i = 0; i < count; i++)
        {
            SpawnBloodDecal(events[i].intersection, events[i].normal);
        }
    }

    private void SpawnBloodDecal(Vector3 position, Vector3 normal)
    {
        Quaternion rotation = Quaternion.LookRotation(-normal);

        rotation *= Quaternion.AngleAxis(
            Random.Range(0f, 360f),
            Vector3.forward
        );

        DecalProjector decal = Instantiate(
            _bloodDecalPrefab,
            position - normal * 0.1f,
            rotation
        );

        float size = Random.Range(_minDecalSize, _maxDecalSize);

        decal.size = new Vector3(
            size,
            size,
            decal.size.z
        );
    }
}