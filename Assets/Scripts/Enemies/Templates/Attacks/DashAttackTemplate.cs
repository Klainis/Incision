using Incision.Enemies.Interfaces;
using Incision.Enemies.Runtime.Attacks;
using UnityEngine;

namespace Incision.Enemies.Templates
{
    [CreateAssetMenu(menuName = "Enemies/Attacks/Dash", fileName = "DashTemplate")]
    public class DashAttackTemplate : AttackTemplate
    {
        [Header("Dash specific parameters")] 
        [SerializeField, Range(0f, 3f)] private float startDelay = 0.1f;
        [SerializeField, Range(0f, 3f)] private float endDelay = 0.1f;
        [SerializeField] private float duration = 0.5f;
        [SerializeField] private float speed = 3f;
        
        public float StartDelay => startDelay;
        public float EndDelay => endDelay;
        public float Duration => duration;
        public float Speed => speed;

        public override IEnemyAttack CreateRuntime()
            => new DashAttack(this);
    }
}