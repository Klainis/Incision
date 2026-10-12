using System;
using UnityEngine;

namespace Incision.Enemies.Components
{
    public class EnemyAnimEvents : MonoBehaviour
    {
        public event Action OnAttackHit;

        public void OnHit() => OnAttackHit?.Invoke();
    }
}
