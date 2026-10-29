using Incision.Enemies.Structs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIHealth : MonoBehaviour
{
    [SerializeField] private Image _healthBarFill;
    [SerializeField] private Image _damageBarFill;
    [SerializeField] private float _delay = 0.4f;
    [SerializeField] private float _speed = 0.5f;

    private float _delayTimer;

    public virtual void TakeDamage(DamageInfo info, float currentHealth, float maxHealth)
    {
        float target = maxHealth > 0f ? Mathf.Clamp01(currentHealth / maxHealth) : 0f;

        if (target < _healthBarFill.fillAmount)
        {
            _delayTimer = _delay;
        }
        else if (target > _damageBarFill.fillAmount)
        {
            _damageBarFill.fillAmount = target;
        }

        _healthBarFill.fillAmount = target;
    }

    private void Update()
    {
        if (_damageBarFill.fillAmount <= _healthBarFill.fillAmount)
            return;

        if (_delayTimer > 0f)
        {
            _delayTimer -= Time.deltaTime;
            return;
        }

        _damageBarFill.fillAmount = Mathf.MoveTowards(
            _damageBarFill.fillAmount, _healthBarFill.fillAmount, _speed * Time.deltaTime);
    }
}
