using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete("Use Incision.Enemies.Components.EnemySpawner instead")]
public class EnemySpawner : MonoBehaviour
{
    private static EnemySpawner _instance;
    public static EnemySpawner Instance { get { return _instance; } }

    [Serializable]
    public class EnemyType
    {
        public GameObject prefab;
        public int count;
    }

    [Serializable]
    public class Wave
    {
        public int minute;
        public List<EnemyType> enemies;
    }

    [Header("Waves")]
    [SerializeField] private List<Wave> _waves;

    [Header("Spawn Settings")]
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _minSpawnRadius = 15f;
    [SerializeField] private float _maxSpawnRadius = 20f;
    [SerializeField] private int _batchSize = 5;
    [SerializeField] private float _initialDelayBetweenBatches = 5f;

    [Header("Limits")]
    [SerializeField] private int _maxEnemies = 300;
    [SerializeField] private float _despawnDistance = 100f;

    [Header("Layer Settings")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _raycastHeight = 200f;

    private bool _readyToStart = false;

    private float _timer;
    private float _delayBetweenBatches;
    private int _currentWaveIndex = 0;
    private Vector3 _enemyTargetPositionForEffect;
    private float _enemyHeight;
    private bool _isRising = false;
    private List<GameObject> _activeEnemies = new List<GameObject>();

    private Coroutine _spawnWaveCoroutine;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        _instance = this;
    }

    private void Start()
    {
        _timer = 0;
        _delayBetweenBatches = _initialDelayBetweenBatches;
        StartCoroutine(Waiter());
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        int currentMinute = Mathf.FloorToInt(_timer / 60f);

        if (!_readyToStart)
        {
            return;
        }

        if (_currentWaveIndex < _waves.Count && currentMinute >= _waves[_currentWaveIndex].minute)
        {
            Wave wave = _waves[_currentWaveIndex];

            _currentWaveIndex++;
            _delayBetweenBatches = Mathf.Max(0.2f, _initialDelayBetweenBatches / _currentWaveIndex);

            if (_spawnWaveCoroutine != null)
            {
                StopCoroutine(_spawnWaveCoroutine);
            }
            _spawnWaveCoroutine = StartCoroutine(SpawnWave(wave));
        }

        CleanupEnemies();
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        var batchCount = 60 / _delayBetweenBatches;
        int enemyCountInWave = 0;

        foreach (var enemy in wave.enemies)
        {
            enemyCountInWave += enemy.count;
        }

        int batch = (int)Math.Ceiling(enemyCountInWave / batchCount);

        batch = batch < 0 ? 1 : batch;

        foreach (var enemy in wave.enemies)
        {
            int spawned = 0;

            while (spawned < enemy.count)
            {
                int currentEnemyBatch = Math.Min(batch, enemy.count - spawned);

                for (int i = 0; i < currentEnemyBatch; i++)
                {
                    SpawnEnemy(enemy.prefab);
                }

                spawned += currentEnemyBatch;
                yield return new WaitForSeconds(_delayBetweenBatches);
            }
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        if (_activeEnemies.Count >= _maxEnemies)
        {
            return;
        }

        Vector3 spawnPosition = GetSpawnPosition(enemyPrefab, 0);

        if (spawnPosition == Vector3.zero)
        {
            Debug.Log("�� ����� ����� ������ ��� �����");
            return;
        }

        var enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        Vector3 direction = (_playerTransform.position - enemy.transform.position).normalized;

        if (direction.magnitude > 0.01f)
        {
            direction.y = 0;
            enemy.transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private Vector3 GetSpawnPosition(GameObject enemyPrefab, int returnsCount)
    {
        if (returnsCount >= 10)
        {
            Debug.LogWarning("���� �� ����� ��������� ����� �� 10 �������. ������� � ��������� �����.");
            return Vector3.zero;
        }

        var capsule = enemyPrefab.GetComponent<CapsuleCollider>();
        _enemyHeight = capsule != null ? capsule.height : 2.81f;

        Vector2 dir = UnityEngine.Random.insideUnitCircle;
        float distance = UnityEngine.Random.Range(_minSpawnRadius, _maxSpawnRadius);
        Vector3 playerPosition = _playerTransform.position;

        Vector3 origin = new Vector3(
                            playerPosition.x + dir.x * distance,
                            playerPosition.y + 100,
                            playerPosition.z + dir.y * distance
                            );

        RaycastHit hit;
        if (Physics.Raycast(origin, Vector3.down, out hit, _raycastHeight, _groundLayer))
        {
            if (hit.normal.y < 0.2f)
            {
                return GetSpawnPosition(enemyPrefab, returnsCount + 1);
            }

            Vector3 checkPos = hit.point;
            checkPos.y += _enemyHeight * 1.2f;

            if (Physics.CheckSphere(checkPos, 2f, ~_groundLayer))
            {
                return GetSpawnPosition(enemyPrefab, returnsCount + 1);
            }

            Vector3 enemyPosition = hit.point + Vector3.up * (_enemyHeight);
            return enemyPosition;
        }
        else
        {
            return GetSpawnPosition(enemyPrefab, returnsCount + 1);
        }
    }

    private void CleanupEnemies()
    {
        for (int i = _activeEnemies.Count - 1; i >= 0; i--)
        {
            var enemy = _activeEnemies[i];

            if (enemy == null)
            {
                _activeEnemies.RemoveAt(i);
                continue;
            }

            if (_activeEnemies.Count > _maxEnemies)
            {
                float distance = Vector3.Distance(_playerTransform.position, enemy.transform.position);

                if (distance > _despawnDistance)
                {
                    Debug.Log("���� �����������");
                    Destroy(enemy);
                    _activeEnemies.RemoveAt(i);
                }
            }
        }
    }

    private IEnumerator Waiter()
    {
        yield return new WaitForSeconds(1.5f);
        _readyToStart = true;
    }
}
