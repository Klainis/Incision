using Incision.Enemies.Structs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Incision
{
    public class EnemyHealthUI : UIHealth
    {
        [SerializeField] private GameObject _barObject;

        private void OnEnable()
        {
            _barObject.SetActive(false);
        }

        public override void TakeDamage(DamageInfo info, float currentHealth, float maxHealth)
        {
            _barObject.SetActive(true);

            base.TakeDamage(info, currentHealth, maxHealth);
        }
    }
}
