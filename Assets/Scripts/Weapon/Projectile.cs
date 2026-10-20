using Incision.Enemies.Interfaces;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(Rigidbody))]
public abstract class Projectile : MonoBehaviour
{
    [Header("Common")]
    [SerializeField] private ProjectileDisposeType _disposeType = ProjectileDisposeType.OnTargetCollision;
    [SerializeField] private float _destroyTime = 6f;

    [Header("Rigidbody")]
    [SerializeField] private Rigidbody _projectileRigidbody;

    private bool _isProjectileDisposed;
    protected bool _isCrit;

    protected float _damage = 10f;

    public float Damage => _damage;
    public bool IsCrit => _isCrit;
    public ProjectileDisposeType DisposeType => _disposeType;
    public Rigidbody Rigidbody => _projectileRigidbody;

    private void Start()
    {
        Destroy(gameObject, _destroyTime);
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

        Destroy(gameObject);
    }

    protected virtual void OnProjectileDispose() { }
    protected virtual void OnAnyCollision() { }
    protected virtual void OnOtherCollision() { }
    protected virtual void OnTargetCollision(Collision collision, IDamageable damageable) { }
}
