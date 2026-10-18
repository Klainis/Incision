using Incision.Enemies.Components;
using Incision.Enemies.Structs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffecter : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Health _enemyHealth;

    [Header("Effects")]
    [SerializeField] private ParticleSystem _bloodExplosionEffect;

    private ParticleSystem _bloodEffectInstance;

    private void OnEnable()
    {
        _enemyHealth.OnChanged += DamageFlash;
        _enemyHealth.OnDied += DieBloodExplosion;
    }

    private void OnDisable()
    {
        _enemyHealth.OnChanged -= DamageFlash;
        _enemyHealth.OnDied -= DieBloodExplosion;
    }

    private void DamageFlash(DamageInfo info, float currentHealth, float maxHealth)
    {
        //TODO
    }

    private void DieBloodExplosion(Health health)
    {
        _bloodEffectInstance = Instantiate(_bloodExplosionEffect, transform.position, Quaternion.identity);
    }
}
