using Incision.Enemies.Components;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    [SerializeField] private PlayerStats _playerStatsData;
    [SerializeField] private UIHealth _healthBar;
    
    private void Start()
    {
        Initialize(_playerStatsData.CreateRuntime());
    }

    private void OnEnable()
    {
        OnChanged += _healthBar.TakeDamage;
        OnDied += Die;
    }

    private void OnDisable()
    {
        OnChanged -= _healthBar.TakeDamage;
        OnDied -= Die;
    }

    private void Die(Health health)
    {
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        //DieEffect();
    }
}
