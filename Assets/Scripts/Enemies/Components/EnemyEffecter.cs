using Incision.Enemies.Components;
using Incision.Enemies.Structs;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class EnemyEffector : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] private ParticleSystem _bloodExplosionEffect;
    [SerializeField] private ParticleSystem _bloodHitEffect;

    private Health _enemyHealth;
    [Inject] private IObjectResolver _resolver;

    [Inject] private UIEffectManager _uiEffectManager;

    public void Initialize(Health health)
    {
        _enemyHealth = health;

        _enemyHealth.OnChanged += OnDamaged;
        _enemyHealth.OnDied += DieBloodExplosion;
    }

    private void OnDisable()
    {
        _enemyHealth.OnChanged -= OnDamaged;
        _enemyHealth.OnDied -= DieBloodExplosion;
    }

    private void OnDamaged(DamageInfo info, float curr, float max)
    {
        _uiEffectManager.ShowDamagePopup(info.Point, info.Amount, info.IsCritical);

        DamageFlash();
        HitBloodEffect(info);
    }

    private void DamageFlash()
    {
        //TODO
    }

    private void HitBloodEffect(DamageInfo info)
    {
        if (_bloodHitEffect != null)
            _resolver.Instantiate(_bloodHitEffect, info.Point, Quaternion.identity);
    }

    private void DieBloodExplosion(Health health)
    {
        _resolver.Instantiate(_bloodExplosionEffect, transform.position, Quaternion.identity);
    }
}
