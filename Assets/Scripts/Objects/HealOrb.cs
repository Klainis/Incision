using Incision.Enemies.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Incision
{
    public class HealOrb : MonoBehaviour
    {
        [SerializeField] private HealOrbTemplate _healOrbData; 

        private float _healAmount = 10f;

        private float _attractDistance = 8f;
        private float _pickupDistance = 0.6f;
        private float _startSpeed = 4f;
        private float _acceleration = 40f;
        private float _maxSpeed = 40f;
        private float _activationDelay = 0.4f;
        private Vector3 _targetOffset = new Vector3(0f, 0f, 0f);
        
        private float _lifeTime = 30f;

        //Drop
        private Vector2 _dropHorizontalSpeed = new Vector2(3f, 8f);
        private Vector2 _dropUpSpeed = new Vector2(8f, 14f);
        private float _dropGravity = 30f;
        private float _bounciness = 0.45f;
        private float _groundFriction = 0.6f;
        private int _maxBounces = 3;
        private float _minBounceSpeed = 2f;
        private float _collisionRadius = 0.25f;
        private LayerMask _groundMask = ~0;

        //Idle
        private float _spinSpeed = 180f;
        private float _bobAmplitude = 0.15f;
        private float _bobFrequency = 1.5f;

        private Transform _player;
        private Health _playerHealth;

        private float _spawnTime;
        private float _currentSpeed;

        private bool _isAttracted;
        private bool _isCollected;
        private bool _attractedDuringDrop;

        public void Initialize()
        {
            if (_healOrbData == null) return;

            _healAmount = _healOrbData.HealAmount;
            _attractDistance = _healOrbData.AttributeactDistance;
            _pickupDistance = _healOrbData.PickupDistance;
            _startSpeed = _healOrbData.StartSpeed;
            _acceleration = _healOrbData.Acceleration;
            _maxSpeed = _healOrbData.MaxSpeed;
            _activationDelay = _healOrbData.ActivationDelay;
            _targetOffset = _healOrbData.TargetOffset;
            _lifeTime = _healOrbData.LifeTime;

            _dropHorizontalSpeed = _healOrbData.DropHorizontalSpeed;
            _dropUpSpeed = _healOrbData.DropUpSpeed;
            _dropGravity = _healOrbData.DropGravity;
            _bounciness = _healOrbData.Bounciness;
            _groundFriction = _healOrbData.GroundFriction;
            _maxBounces = _healOrbData.MaxBounces;
            _minBounceSpeed = _healOrbData.MinBounceSpeed;
            _collisionRadius = _healOrbData.CollisionRadius;
            _groundMask = _healOrbData.GroundMask;

            _spinSpeed = _healOrbData.SpinSpeed;
            _bobAmplitude = _healOrbData.BobAmplitude;
            _bobFrequency = _healOrbData.BobFrequency;
        }

        private void OnEnable()
        {
            Initialize();
        }

        private void Start()
        {
            _spawnTime = Time.time;

            if (_lifeTime > 0f)
                Destroy(gameObject, _lifeTime);

            StartCoroutine(LifeRoutine());
        }

        private IEnumerator LifeRoutine()
        {
            yield return DropRoutine();

            if (!_attractedDuringDrop)
            {
                yield return IdleRoutine();
            }

            yield return AttractRoutine();

            Collect();
        }

        private IEnumerator DropRoutine()
        {
            Vector2 circle = Random.insideUnitCircle.normalized;
            Vector3 horizontalDir = new Vector3(circle.x, 0f, circle.y);

            Vector3 velocity =
                horizontalDir * Random.Range(_dropHorizontalSpeed.x, _dropHorizontalSpeed.y) +
                Vector3.up * Random.Range(_dropUpSpeed.x, _dropUpSpeed.y);

            int bounces = 0;
            float spinDir = Random.value > 0.5f ? 1f : -1f;

            while (true)
            {
                if (IsPlayerInAttractRange())
                {
                    _attractedDuringDrop = true;
                    yield break;
                }

                float dt = Time.deltaTime;

                velocity.y -= _dropGravity * dt;
                Vector3 step = velocity * dt;
                float stepLength = step.magnitude;

                transform.Rotate(0f, _spinSpeed * 2f * spinDir * dt, 0f, Space.World);

                if (stepLength > 0.0001f &&
                    Physics.SphereCast(transform.position, _collisionRadius, step / stepLength,
                        out RaycastHit hit, stepLength, _groundMask, QueryTriggerInteraction.Ignore))
                {
                    transform.position = hit.point + hit.normal * _collisionRadius;

                    // Разложение скорости на нормальную и касательную составляющие
                    Vector3 normalVelocity = Vector3.Project(velocity, hit.normal);
                    Vector3 tangentVelocity = velocity - normalVelocity;

                    velocity = tangentVelocity * (1f - _groundFriction) - normalVelocity * _bounciness;
                    bounces++;

                    if (bounces >= _maxBounces || normalVelocity.magnitude * _bounciness < _minBounceSpeed)
                        yield break;
                }
                else
                {
                    transform.position += step;
                }

                yield return null;
            }
        }

        private IEnumerator IdleRoutine()
        {
            Vector3 basePos = transform.position;
            float phase = Random.value * Mathf.PI * 2f;

            while (true)
            {
                float dt = Time.deltaTime;
                phase += _bobFrequency * Mathf.PI * 2f * dt;

                transform.position = basePos + Vector3.up * (Mathf.Sin(phase) * _bobAmplitude);
                transform.Rotate(0f, _spinSpeed * dt, 0f, Space.World);

                if (IsPlayerInAttractRange())
                    yield break;

                yield return null;
            }
        }

        private IEnumerator AttractRoutine()
        {
            float speed = _startSpeed;

            while (true)
            {
                if (_player == null)
                {
                    Destroy(gameObject);
                    yield break;
                }

                float dt = Time.deltaTime;
                speed = Mathf.Min(speed + _acceleration * dt, _maxSpeed);

                Vector3 targetPoint = _player.position + _targetOffset;
                transform.position = Vector3.MoveTowards(transform.position, targetPoint, speed * dt);
                transform.Rotate(0f, _spinSpeed * 2f * dt, 0f, Space.World);

                if (Vector3.Distance(transform.position, targetPoint) <= _pickupDistance)
                    yield break;

                yield return null;
            }
        }

        private bool IsPlayerInAttractRange()
        {
            if (Time.time - _spawnTime < _activationDelay) return false;
            if (!TryResolvePlayer()) return false;

            float distance = Vector3.Distance(transform.position, _player.position + _targetOffset);
            return distance <= _attractDistance;
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
            if (_isCollected) return;
            _isCollected = true;

            if (_playerHealth != null)
                _playerHealth.Heal(_healAmount);

            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _attractDistance);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _pickupDistance);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _collisionRadius);
        }
    }
}

