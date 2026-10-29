using Incision;
using Incision.Enemies.Components;
using Incision.Enemies.Structs;
using System.Collections;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class EnemyEffector : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private Renderer[] _enemyRenderers;

    [Header("Health")]
    [SerializeField] private EnemyHealthUI _healthBar;

    [Header("Effects")]
    [SerializeField] private ParticleSystem _bloodExplosionEffect;
    [SerializeField] private ParticleSystem _bloodHitEffect;

    [Header("Flash Settings")]
    [SerializeField][Range(0, 1)] private float _flashAmount = 1;
    [SerializeField] private float _flashTime = 0.25f;

    private EnemyHealth _enemyHealth;
    [Inject] private IObjectResolver _resolver;

    private MaterialPropertyBlock _propertyBlock;

    private static readonly int FlashAmountID = Shader.PropertyToID("_FlashAmount");

    [Inject] private UIEffectManager _uiEffectManager;

    private Coroutine _flashCoroutine;

    public void Initialize(EnemyHealth health)
    {
        _enemyHealth = health;

        _enemyHealth.OnChanged += OnDamaged;
        _enemyHealth.OnChanged += _healthBar.TakeDamage;
        _enemyHealth.OnDied += DieBloodExplosion;

        _propertyBlock = new MaterialPropertyBlock();
    }

    private void OnDisable()
    {
        _enemyHealth.OnChanged -= OnDamaged;
        _enemyHealth.OnChanged -= _healthBar.TakeDamage;
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
        if (_flashCoroutine != null)
            StopCoroutine(_flashCoroutine);

        _flashCoroutine = StartCoroutine(DamageFlashRoutine());
    }

    private IEnumerator DamageFlashRoutine()
    {
        SetFlashAmount(_flashAmount);

        yield return new WaitForSeconds(_flashTime);

        SetFlashAmount(0f);
        _flashCoroutine = null;
    }

    private void SetFlashAmount(float amount)
    {
        foreach (Renderer renderer in _enemyRenderers)
        {
            if (renderer == null)
                continue;

            renderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetFloat(FlashAmountID, amount);
            renderer.SetPropertyBlock(_propertyBlock);
        }
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
