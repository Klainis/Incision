using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject _damagePopupPrefab;

    public ParticleSystem _bloodEffect;

    public float maxHealth;

    private float _currentHealt;

    private ParticleSystem _bloodEffectInstance;
    private GameObject _damagePopupInstance;

    private void Start()
    {
        _currentHealt = maxHealth;
    }

    public void ApplyDamage(float damage, bool isCrit)
    {
        UIEffectManager.Instance.ShowDamagePopup(transform.position, damage, isCrit);

        _currentHealt -= damage;

        if (_currentHealt <= 0 )
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);

        DieEffect();
    }

    private void DieEffect()
    {
        _bloodEffectInstance = Instantiate(_bloodEffect, transform.position, Quaternion.identity);
    }
}
