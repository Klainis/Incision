using Incision.Enemies.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Incision
{
    public class HealOrb : MonoBehaviour
    {
        [Header("Heal")]
        [SerializeField] private float _healAmount = 10f;

        [Header("Attraction")]
        [SerializeField] private float _attractDistance = 8f;
        [SerializeField] private float _pickupDistance = 0.6f;
        [SerializeField] private float _startSpeed = 4f;
        [SerializeField] private float _acceleration = 40f;
        [SerializeField] private float _maxSpeed = 40f;
        [SerializeField] private float _activationDelay = 0.4f;
        [SerializeField] private Vector3 _targetOffset = new Vector3(0f, 1f, 0f);

        [Header("Lifetime")]
        [SerializeField] private float _lifeTime = 30f;

        private Transform _player;
        private Health _playerHealth;

        private float _spawnTime;
        private float _currentSpeed;
        private bool _isAttracted;
        private bool _isCollected;

        public void Initialize(float healAmount)
        {
            _healAmount = healAmount;
        }

        private void Start()
        {
            _spawnTime = Time.time;
            _currentSpeed = _startSpeed;

            if (_lifeTime > 0f)
                Destroy(gameObject, _lifeTime);
        }

        private void Update()
        {
            if (_isCollected) return;
            if (Time.time - _spawnTime < _activationDelay) return;
            if (!TryResolvePlayer()) return;

            Vector3 targetPoint = _player.position + _targetOffset;
            float distance = Vector3.Distance(transform.position, targetPoint);

            if (!_isAttracted && distance <= _attractDistance)
                _isAttracted = true;

            if (!_isAttracted) return;

            _currentSpeed = Mathf.Min(_currentSpeed + _acceleration * Time.deltaTime, _maxSpeed);
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, _currentSpeed * Time.deltaTime);

            if (distance <= _pickupDistance)
                Collect();
        }

        private bool TryResolvePlayer()
        {
            if (_player != null && _playerHealth != null) return true;

            PlayerCharacter character = PlayerCharacter.Instance;
            if (character == null) return false;

            _player = character.transform;
            _playerHealth = character.GetComponentInParent<PlayerHealth>();

            return _playerHealth != null;
        }

        private void Collect()
        {
            _isCollected = true;

            _playerHealth.Heal(_healAmount);

            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _attractDistance);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _pickupDistance);
        }
    }
}

