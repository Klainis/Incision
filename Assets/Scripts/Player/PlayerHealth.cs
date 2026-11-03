using Incision;
using Incision.Enemies.Components;
using Incision.Enemies.Structs;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

public class PlayerHealth : Health
{
    private PlayerStats _playerStatsData;
    private PlayerHealthUI _healthBar;

    [Inject] private UIEffectManager _uiEffectManager;
    
    public void InitializePlayerHealth(PlayerStats stats, PlayerHealthUI bar)
    {
        _playerStatsData = stats;
        _healthBar = bar;

        OnDamaged += _healthBar.TakeDamage;
        OnHealed += _healthBar.Heal;
        OnDamaged += TakeDamageEffect;
        OnDied += PlayerDie;

        Initialize(_playerStatsData.CreateRuntime());
        _healthBar.Initialize(_playerStatsData.CurrentHealth, _playerStatsData.MaxHealth, _healthBar.HealthText);
    }

    private void OnDisable()
    {
        OnDamaged -= _healthBar.TakeDamage;
        OnHealed -= _healthBar.Heal;
        OnDamaged -= TakeDamageEffect;
        OnDied -= PlayerDie;
    }

    private void PlayerDie(Health health)
    {
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void TakeDamageEffect(DamageInfo info, float currentHealth, float maxHealth)
    {
        _uiEffectManager.ShowBloodScreen();
    }
}
