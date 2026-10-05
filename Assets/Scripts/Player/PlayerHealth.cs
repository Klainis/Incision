using System.Collections;
using System.Collections.Generic;
using Incision.Enemies.Interfaces;
using Incision.Enemies.Structs;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void TakeDamage(DamageInfo damageInfo)
    {
        _currentHealt -= damageInfo.Amount;

        _healthBar.TakeDamage(_maxHealth, _currentHealt);

        if (_currentHealt <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        //DieEffect();
    }
}
