using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(Rigidbody))]
public abstract class Projectile : MonoBehaviour
{
    [Header("Common")]
    [SerializeField, Min(0f)] private float _damage = 10f;
    [SerializeField] private ProjectileDisposeType _disposeType = ProjectileDisposeType.OnTargetCollision;
    [SerializeField] private GameObject _cardRoot;
    [SerializeField] private float _destroyTime = 6f;

    [Header("Overlap Scale By Distance")]
    [SerializeField] private float _projectileMinScale = 1f;
    [SerializeField] private float _projectileMaxScale = 2f;
    [SerializeField] private float _distanceForChangeScale = 50f;

    [Header("Rigidbody")]
    [SerializeField] private Rigidbody _projectileRigidbody;

    [Header("Effect On Destroy")]
    [SerializeField] private bool _spawnEffectOnDestroy = false;
    [SerializeField] private ParticleSystem _hitEffect;
    [SerializeField] private ParticleSystem _bloodHitEffect;

    [Header("Effects")]
    [SerializeField] private float _cardRotaionSpeed = 180f;
    [SerializeField] private TrailRenderer _trailRenderer;

    [Header("Collision")]
    [SerializeField] private SphereCollider _projectileColliderForEnemy;

    private bool _isProjectileDisposed;
    private bool _canRotationCard = true;
    private bool _isCrit;

    private float _scaleTime = 0f;
    private float _initialColliderRadiusForEnemyScale;
    private float _currentColliderRadiusForEnemyScale;

    private Vector3 _initialPosition;

    public float Damage => _damage;
    public bool IsCrit => _isCrit;
    public ProjectileDisposeType DisposeType => _disposeType;
    public Rigidbody Rigidbody => _projectileRigidbody;

    // Вызывать сразу после Instantiate
    public void Initialize(float damage, bool isCrit)
    {
        _damage = damage;
        _isCrit = isCrit;
    }

    private void Start()
    {
        if (_trailRenderer != null)
        {
            _trailRenderer.enabled = true;
        }
        else
        {
            Debug.LogError("[Projectile.cs] : Отсутствует компонент TrailRenderer", this);
        }

        _initialPosition = transform.position;
        _initialColliderRadiusForEnemyScale = _projectileColliderForEnemy.radius;
        _currentColliderRadiusForEnemyScale = _initialColliderRadiusForEnemyScale;

        // Снаряд, который ни во что не попал, не живёт вечно
        Destroy(gameObject, _destroyTime);
    }

    private void Update()
    {
        _scaleTime += Time.deltaTime;

        // Изменение размера коллайдера
        if (_scaleTime >= 1)
        {
            _scaleTime = 0;

            float currentDistance = Vector3.Distance(_initialPosition, transform.position);
            if (currentDistance >= _distanceForChangeScale)
            {
                ChangeProjectileScale();
            }
        }

        // Вращение карты
        if (_canRotationCard && _cardRoot != null)
        {
            _cardRoot.transform.Rotate(0, _cardRotaionSpeed * Time.deltaTime, 0, Space.Self);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isProjectileDisposed)
            return;

        if (collision.gameObject.TryGetComponent(out IDamageable damageable) && !collision.gameObject.CompareTag("Player"))
        {
            OnTargetCollision(collision, damageable);

            if (_disposeType == ProjectileDisposeType.OnTargetCollision)
            {
                DisposeProjectile();
            }
        }
        else
        {
            _canRotationCard = false;
            OnOtherCollision();
        }

        OnAnyCollision();

        if (_disposeType == ProjectileDisposeType.OnAnyCollision)
        {
            DisposeProjectile();
        }
    }

    public void DisposeProjectile()
    {
        if (_isProjectileDisposed)
            return;

        _isProjectileDisposed = true;

        OnProjectileDispose();
        SpawnEffectOnDestroy();

        Destroy(gameObject);
    }

    private void ChangeProjectileScale()
    {
        _currentColliderRadiusForEnemyScale = _projectileMaxScale;
        _projectileColliderForEnemy.radius = _currentColliderRadiusForEnemyScale;
    }

    protected virtual void OnProjectileDispose() { }
    protected virtual void OnAnyCollision() { }
    protected virtual void OnOtherCollision() { }
    protected virtual void OnTargetCollision(Collision collision, IDamageable damageable) { }

    private void SpawnEffectOnDestroy()
    {
        if (!_spawnEffectOnDestroy)
            return;

        if (_hitEffect != null)
            Instantiate(_hitEffect, transform.position, transform.rotation);

        if (_bloodHitEffect != null)
            Instantiate(_bloodHitEffect, transform.position, transform.rotation);
    }
}
