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

        public override void TakeDamage(DamageInfo info, float currentHealth, float maxHealth)
        {
            base.TakeDamage(info, currentHealth, maxHealth);

            _healthText.text = $"{currentHealth}/{maxHealth}";
        }
    }
}
