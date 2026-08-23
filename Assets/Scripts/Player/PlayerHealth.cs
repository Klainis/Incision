using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerStats _playerStatsData;
    [SerializeField] private UIHealth _healthBar;

    private float _maxHealth;
    public float _currentHealt;

    private void Start()
    {
        _maxHealth = _playerStatsData.MaxHP;
        _currentHealt = _maxHealth;
    }

    public void ApplyDamage(float damage)
    {
        _currentHealt -= damage;

        _healthBar.TakeDamage(_maxHealth, _currentHealt);

        if (_currentHealt <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);

        //DieEffect();
    }
}
