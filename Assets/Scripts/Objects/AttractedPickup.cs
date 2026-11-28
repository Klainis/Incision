using System.Collections;
using UnityEngine;

namespace Incision
{
    public abstract class AttractedPickup : MonoBehaviour
    {
        [SerializeField] private AttractedPickupEffectTemplate _effectTemplate;

        private float _attractDistance = 8f;
        private float _pickupDistance = 0.8f;
        private float _startSpeed = 30f;
        private float _acceleration = 50f;
        private float _maxSpeed = 80f;
        private float _activationDelay = 0.25f;
        private Vector3 _targetOffset = Vector3.zero;
        private float _lifeTime = 30f;
        private Vector2 _dropHorizontalSpeed = new Vector2(4f, 6f);
        private Vector2 _dropUpSpeed = new Vector2(8f, 16f);
        private float _dropGravity = 50f;
        private float _bounciness = 0.45f;
        private float _groundFriction = 0.7f;
        private int _maxBounces = 2;
        private float _minBounceSpeed = 2f;
        private float _collisionRadius = 0.3f;
        private LayerMask _groundMask = 1025;
        private float _spinSpeed = 180f;
        private float _bobAmplitude = 0f;
        private float _bobFrequency = 1.5f;

        private Transform _player;

        private float _spawnTime;

        private bool _isCollected;
        private bool _attractedDuringDrop;

        private TrailRenderer _trailRenderer;


        public void Initialize()
        {
            if (_effectTemplate is null)
                return;

            _attractDistance = _effectTemplate.AttractDistance;
            _pickupDistance = _effectTemplate.PickupDistance;
            _startSpeed = _effectTemplate.StartSpeed;
            _acceleration = _effectTemplate.Acceleration;
            _maxSpeed = _effectTemplate.MaxSpeed;
            _activationDelay = _effectTemplate.ActivationDelay;
            _targetOffset = _effectTemplate.TargetOffset;
            _lifeTime = _effectTemplate.LifeTime;

            _dropHorizontalSpeed = _effectTemplate.DropHorizontalSpeed;
            _dropUpSpeed = _effectTemplate.DropUpSpeed;
            _dropGravity = _effectTemplate.DropGravity;
            _bounciness = _effectTemplate.Bounciness;
            _groundFriction = _effectTemplate.GroundFriction;
            _maxBounces = _effectTemplate.MaxBounces;
            _minBounceSpeed = _effectTemplate.MinBounceSpeed;
            _collisionRadius = _effectTemplate.CollisionRadius;
            _groundMask = _effectTemplate.GroundMask;

            _spinSpeed = _effectTemplate.SpinSpeed;
            _bobAmplitude = _effectTemplate.BobAmplitude;
            _bobFrequency = _effectTemplate.BobFrequency;
        }

        protected virtual bool DropsOnSpawn => true;
        protected virtual float LifeTime => _lifeTime;

        protected void OnEnable()
        {
            Initialize();
        }

        protected void Start()
        {
            _trailRenderer = GetComponent<TrailRenderer>();
            if (_trailRenderer != null)
                _trailRenderer.enabled = true;

            _spawnTime = Time.time;

            if (LifeTime > 0f)
                Destroy(gameObject, LifeTime);

            StartCoroutine(LifeRoutine());
        }

        protected virtual IEnumerator LifeRoutine()
        {
            if (DropsOnSpawn)
                yield return DropRoutine();

            if (!_attractedDuringDrop)
            {
                yield return IdleRoutine();
            }

            yield return AttractRoutine();

            if (_player != null)
                Collect();
            else
                Destroy(gameObject);
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
                    yield break;

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
            _player = ResolvePlayerTarget(_player, _attractDistance);
            if (_player == null) return false;

            float distance = Vector3.Distance(transform.position, _player.position + _targetOffset);
            return distance <= _attractDistance;
        }

        protected abstract Transform ResolvePlayerTarget(Transform currentTarget, float attractDistance);

        protected void Collect()
        {
            if (_isCollected) return;
            _isCollected = true;

            if (_trailRenderer != null)
                _trailRenderer.enabled = false;
            Destroy(gameObject);
            OnPickedUp();
        }

        protected virtual void OnPickedUp() {}

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
