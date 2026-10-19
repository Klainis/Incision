using System;
using Incision.Enemies.Interfaces;
using Incision.Enemies.Structs;
using UnityEngine;

[Obsolete("Use Health component instead")]
    public class EnemyHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private GameObject _damagePopupPrefab;

        public ParticleSystem _bloodEffect;

        public float maxHealth;

        private float _currentHealt;

        private ParticleSystem _bloodEffectInstance;
        private GameObject _damagePopupInstance;

        private void Start()
        {
            _currentHealt = maxHealth;
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            // UIEffectManager.Instance.ShowDamagePopup(transform.position, damageInfo.Amount);

            _currentHealt -= damageInfo.Amount;

            if (_currentHealt <= 0 )
            {
                Die();
            }
        }

        private void Die()
        {
            DieEffect();
            Destroy(gameObject);
        }

        private void DieEffect()
        {
            _bloodEffectInstance = Instantiate(_bloodEffect, transform.position, Quaternion.identity);
        }
    }
