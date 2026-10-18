using Incision.Enemies.Components;
using Incision.Enemies.Structs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class EnemyEffecter : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] private ParticleSystem _bloodExplosionEffect;
    [SerializeField] private ParticleSystem _bloodHitEffect;

    private Health _enemyHealth;

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
            Instantiate(_bloodHitEffect, info.Point, Quaternion.identity);
    }

    private void DieBloodExplosion(Health health)
    {
        Instantiate(_bloodExplosionEffect, transform.position, Quaternion.identity);
    }
}
