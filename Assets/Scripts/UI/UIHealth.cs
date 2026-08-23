using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    [SerializeField] private Image _healthBarFill;
    [SerializeField] private TextMeshProUGUI _healthText;

    public void TakeDamage(float maxHealth, float currentHealth)
    {
        if (currentHealth < 0) currentHealth = 0;

        _healthBarFill.fillAmount = currentHealth / maxHealth;

        _healthText.text = $"{currentHealth}/{maxHealth}";
    }
}
