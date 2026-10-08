using System;
using UnityEngine;
using Incision.Enemies.Interfaces;
using Incision.Enemies.Runtime;
using Incision.Enemies.Structs;
using Incision.Enemies.Structs.Events;
using Incision.Interfaces;
using VContainer;

namespace Incision.Enemies.Components
{
    public class Health : MonoBehaviour, IDamageable
    {
        public event Action<Health> OnDied;
        public event Action<float, float> OnChanged;

        private Stats _stats;
        [Inject] private IEventBus _eventBus;

        public void Initialize(Stats stats)
        {
            _stats = stats;
            _stats.CurrentHealth = _stats.MaxHealth;
        }

        public void TakeDamage(DamageInfo info)
        {
            if (_stats == null || _stats.CurrentHealth <= 0) return;

            float damage = Mathf.Max(0, info.Amount - _stats.Armor);
            _stats.CurrentHealth -= damage;
            OnChanged?.Invoke(_stats.CurrentHealth, _stats.MaxHealth);
            _eventBus.Publish(new OnCreatureDamaged(info));

            if (_stats.CurrentHealth <= 0)
                Die(info);
        }

        private void Die(DamageInfo info)
        {
            OnDied?.Invoke(this);
            _eventBus.Publish(new OnCreatureDied(info));
        }
    }
}