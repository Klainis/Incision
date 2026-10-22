using UnityEngine;

namespace Incision
{
    public abstract class CardProjectile : Projectile
    {
        [Header("Common")]
        [SerializeField] private GameObject _cardRoot;

        [Header("Overlap Scale By Distance")]
        [SerializeField] private float _projectileMinScale = 1f;
        [SerializeField] private float _projectileMaxScale = 2f;
        [SerializeField] private float _distanceForChangeScale = 50f;

        [Header("Effect On Destroy")]
        [SerializeField] private bool _spawnEffectOnDestroy = false;
        [SerializeField] private ParticleSystem _hitEffect;

        [Header("Effects")]
        [SerializeField] private float _cardRotaionSpeed = 180f;
        [SerializeField] private TrailRenderer _trailRenderer;

        [Header("Collision")]
        [SerializeField] private SphereCollider _projectileColliderForEnemy;

        private bool _canRotationCard = true;

        private float _scaleTime = 0f;
        private float _initialColliderRadiusForEnemyScale;
        private float _currentColliderRadiusForEnemyScale;

        private Vector3 _initialPosition;

        private CardData _cardData;

        public CardData CardData => _cardData;

        protected override void OnOtherCollision()
        {
            _canRotationCard = false;
        }

        public void Initialize(float damage, bool isCrit, CardData cardData)
        {
            _cardData = cardData;
            _name = gameObject.name;
            _damage = damage;
            _isCrit = isCrit;
        }

        protected override void Start()
        {
            base.Start();

            if (_trailRenderer != null)
            {
                _trailRenderer.enabled = true;
            }
            else
            {
                Debug.LogError("[Projectile.cs] : Absent TrailRenderer", this);
            }

            _initialPosition = transform.position;
            _initialColliderRadiusForEnemyScale = _projectileColliderForEnemy.radius;
            _currentColliderRadiusForEnemyScale = _initialColliderRadiusForEnemyScale;
        }

        private void Update()
        {
            _scaleTime += Time.deltaTime;

            if (_scaleTime >= 1)
            {
                _scaleTime = 0;

                var currentDistance = Vector3.Distance(_initialPosition, transform.position);
                if (currentDistance >= _distanceForChangeScale)
                {
                    ChangeProjectileScale();
                }
            }

            if (_canRotationCard && _cardRoot != null)
            {
                _cardRoot.transform.Rotate(0, _cardRotaionSpeed * Time.deltaTime, 0, Space.Self);
            }
        }

        private void ChangeProjectileScale()
        {
            _currentColliderRadiusForEnemyScale = _projectileMaxScale;
            _projectileColliderForEnemy.radius = _currentColliderRadiusForEnemyScale;
        }

        public void SpawnEffectOnDestroy()
        {
            if (!_spawnEffectOnDestroy)
                return;

            if (_hitEffect != null)
                Instantiate(_hitEffect, transform.position, transform.rotation);
        }
    }
}
