using Incision.Enemies.Structs;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Incision
{
    public class PlayerHealthUI : UIHealth
    {
        [SerializeField] private TextMeshProUGUI _healthText;

        public TextMeshProUGUI HealthText => _healthText;

        public void InitializePlayerHealthUI(float currentHealth, float maxHealth, TextMeshProUGUI healthText)
        {
            Initialize(currentHealth, maxHealth, healthText);
        }

        public override void TakeDamage(DamageInfo info, float currentHealth, float maxHealth)
        {
            base.TakeDamage(info, currentHealth, maxHealth);

            _healthText.text = $"{currentHealth}/{maxHealth}";
        }

        public override void Heal(float currentHealth, float maxHealth)
        {
            base.Heal(currentHealth, maxHealth);

            _healthText.text = $"{currentHealth}/{maxHealth}";
        }
    }
}
