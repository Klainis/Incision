using System;
using Incision.Data;
using UnityEngine;
using Incision.Enemies.Interfaces;
using Incision.Enemies.Runtime;
using Incision.Enemies.Structs;
using Incision.Enemies.Structs.Events;
using Incision.Interfaces;
using VContainer;

namespace Incision.Enemies.Components
{
    public class Health : MonoBehaviour, IDamageable, IHealable
    {
        public event Action<Health> OnDied;
        public event Action<DamageInfo, float, float> OnDamaged;
        public event Action<float, float> OnHealed;

        private Stats _stats;
        [Inject] private IEventBus _eventBus;

        public void Initialize(Stats stats)
        {
            _stats = stats;
            if (_stats.CurrentHealth > _stats.MaxHealth)
                _stats.CurrentHealth = _stats.MaxHealth;
        }

        public void TakeDamage(DamageInfo info)
        {
            if (_stats == null || _stats.CurrentHealth <= 0) return;

            float damage = Mathf.Max(0, info.Amount - _stats.Armor);
            _stats.CurrentHealth -= damage;
            OnDamaged?.Invoke(info, _stats.CurrentHealth, _stats.MaxHealth);
            _eventBus.Publish(new OnCreatureDamaged(info));

            if (_stats.CurrentHealth <= 0)
                Die(info);
        }

        public void Heal(float amount)
        {
            if (_stats == null || _stats.CurrentHealth <= 0 || amount <= 0f) return;

            _stats.CurrentHealth = Mathf.Min(_stats.CurrentHealth + amount, _stats.MaxHealth);
            OnHealed?.Invoke(_stats.CurrentHealth, _stats.MaxHealth);
        }

        private void Die(DamageInfo info)
        {
            OnDied?.Invoke(this);
            _eventBus.Publish(new OnCreatureDied(info));
        }
    }
}