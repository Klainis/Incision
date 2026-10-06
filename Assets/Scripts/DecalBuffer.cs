using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalBuffer : MonoBehaviour
{
    public static DecalBuffer Instance { get; private set; }

    [SerializeField] private DecalProjector _decalPrefab;
    [SerializeField] private int _maxDecals = 80;

    private readonly Queue<DecalProjector> _decals = new Queue<DecalProjector>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void SpawnDecal(Vector3 position, Vector3 normal, float size)
    {
        Quaternion rotation = Quaternion.LookRotation(-normal);
        rotation *= Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);

        Vector3 spawnPosition = position - normal * 0.1f;

        DecalProjector decal = GetDecal();
        if (decal == null) return;

        decal.transform.SetPositionAndRotation(spawnPosition, rotation);
        decal.size = new Vector3(size, size, decal.size.z);

        _decals.Enqueue(decal);
    }

    private DecalProjector GetDecal()
    {
        while (_decals.Count >= _maxDecals)
        {
            DecalProjector oldest = _decals.Dequeue();

            if (oldest != null)
                return oldest;
        }

        if (_decalPrefab == null)
        {
            return null;
        }

        return Instantiate(_decalPrefab, transform);
    }
}
