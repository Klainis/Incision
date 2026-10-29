using Incision;
using Incision.Enemies.Components;
using Incision.Enemies.Structs;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

public class PlayerHealth : Health
{
    [SerializeField] private PlayerStats _playerStatsData;
    [SerializeField] private PlayerHealthUI _healthBar;

    [Inject] private UIEffectManager _uiEffectManager;
    
    private void Start()
    {
        Initialize(_playerStatsData.CreateRuntime());
    }

    private void OnEnable()
    {
        OnChanged += _healthBar.TakeDamage;
        OnChanged += TakeDamageEffect;
        OnDied += PlayerDie;
    }

    private void OnDisable()
    {
        OnChanged -= _healthBar.TakeDamage;
        OnChanged -= TakeDamageEffect;
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
