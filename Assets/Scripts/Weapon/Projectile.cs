using Incision.Enemies.Interfaces;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(Rigidbody))]
public abstract class Projectile : MonoBehaviour
{
    [Header("Common")]
    [SerializeField, Min(0f)] protected float _damage = 10f;
    [SerializeField] private ProjectileDisposeType _disposeType = ProjectileDisposeType.OnTargetCollision;
    [SerializeField] private GameObject _cardRoot;
    [SerializeField] private float _destroyTime = 6f;

    [Header("Overlap Scale By Distance")]
    [SerializeField] private float _projectileMinScale = 1f;
    //[SerializeField] private float _projectileMiddleScaleDistance = 1.2f;
    [SerializeField] private float _projectileMaxScale = 2f;
    [SerializeField] private float _distanceForChangeScale = 50f;
    //�������� ���� ������������

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

    private float _time = 0f;
    private float _scaleTime = 0f;
    private float _initialColliderRadiusForEnemyScale;
    private float _currentColliderRadiusForEnemyScale;

    private Vector3 _initialPosition;

    private ParticleSystem _hitEffectInstance;
    private ParticleSystem _bloodHitEffectInstance;

    public float Damage { get { return _damage; } }
    public ProjectileDisposeType DisposeType { get { return _disposeType; } }
    public Rigidbody Rigidbody { get { return _projectileRigidbody; } }

    private void Start()
    {
        if (_trailRenderer != null && !_trailRenderer.enabled)
        {
            _trailRenderer.enabled = true;
        }
        else if (_trailRenderer == null)
        {
            Debug.LogError("[Projectile.cs] : ����������� ��������� TrailRenderer");
        }

        _initialPosition = transform.position;
        _initialColliderRadiusForEnemyScale = _projectileColliderForEnemy.radius;
        _currentColliderRadiusForEnemyScale = _initialColliderRadiusForEnemyScale;
    }


    private void Update()
    {
        _scaleTime += Time.deltaTime;

        //��������� ������� ����������
        if (_scaleTime >= 1)
        {
            _scaleTime = 0;

            var currentDistance = Vector3.Distance(_initialPosition, transform.position);
            if (currentDistance >= _distanceForChangeScale)
            {
                ChangeProjectileScale();
            }
        }

        if (_canRotationCard)
        {
            _cardRoot?.transform.Rotate(0, _cardRotaionSpeed * Time.deltaTime, 0, Space.Self);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isProjectileDisposed)
            return;

        if (collision.gameObject.TryGetComponent(out IDamageable damageable) && collision.gameObject.tag != "Player")
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
        OnProjectileDispose();

        SpawnEffectOnDestroy();

        Destroy(gameObject);

        _isProjectileDisposed = true;
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
        _hitEffectInstance = Instantiate(_hitEffect, transform.position, transform.rotation);
        _bloodHitEffectInstance = Instantiate(_bloodHitEffect, transform.position, transform.rotation);
    }
}
