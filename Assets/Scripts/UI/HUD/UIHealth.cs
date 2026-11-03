using Incision;
using Incision.Enemies.Structs;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIHealth : MonoBehaviour
{
    [SerializeField] private Image _healthBarFill;
    [SerializeField] private Image _damageBarFill;
    [SerializeField] private HealthBarDamageTemplate _damageBarData;

    private const float p = 10f;

    private float _delay = 0.4f;
    private float _damageBarDuration = 0.5f;

    private Coroutine _damageBarCoroutine;

    public void Initialize(float currentHealth, float maxHealth, TextMeshProUGUI healthText = null)
    {
        float target = maxHealth > 0f ? Mathf.Clamp01(currentHealth / maxHealth) : 0f;
        _healthBarFill.fillAmount = target;
        _damageBarFill.fillAmount = target;

        _delay = _damageBarData.DamageBarDelay;
        _damageBarDuration = _damageBarData.DamageBarDuration;

        if (healthText != null ) healthText.text = $"{currentHealth}/{maxHealth}";
    }

    public virtual void TakeDamage(DamageInfo info, float currentHealth, float maxHealth)
    {
        SetHealth(currentHealth, maxHealth);
    }

    public virtual void Heal(float currentHealth, float maxHealth)
    {
        SetHealth(currentHealth, maxHealth);
    }

    public void SetHealth(float currentHealth, float maxHealth)
    {
        float target = maxHealth > 0f ? Mathf.Clamp01(currentHealth / maxHealth) : 0f;

        if (target < _healthBarFill.fillAmount)
        {
            StartDamageBarAnimation();
        }
        else if (target > _damageBarFill.fillAmount)
        {
            _damageBarFill.fillAmount = target;
        }

        _healthBarFill.fillAmount = target;
    }

    private void StartDamageBarAnimation()
    {
        if (_damageBarCoroutine != null)
            StopCoroutine(_damageBarCoroutine);

        _damageBarCoroutine = StartCoroutine(UpdateDamageBar());
    }

    private IEnumerator UpdateDamageBar()
    {
        yield return new WaitForSeconds(_delay);

        float elapsedTime = 0;
        float currentDamageFill = _damageBarFill.fillAmount;

        while (elapsedTime <= _damageBarDuration)
        {
            elapsedTime += Time.deltaTime;

            currentDamageFill = Mathf.MoveTowards(_damageBarFill.fillAmount, _healthBarFill.fillAmount, elapsedTime / (_damageBarDuration * p));
            _damageBarFill.fillAmount = currentDamageFill;

            yield return null;
        }

        _damageBarCoroutine = null;
    }
}
