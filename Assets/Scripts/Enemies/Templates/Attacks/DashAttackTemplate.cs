using Incision.Enemies.Interfaces;
using Incision.Enemies.Runtime.Attacks;
using UnityEngine;

namespace Incision.Enemies.Templates
{
    [CreateAssetMenu(menuName = "General/Enemy/Attacks/Dash", fileName = "DashTemplate")]
    public class DashAttackTemplate : AttackTemplate
    {
        [Header("Dash specific parameters")] 
        [SerializeField, Range(1f, 10f)] private float distance = 5f;
        [SerializeField, Range(0f, 10f)] private float jumpHeight = 5f;
        [SerializeField, Min(0f)] private float duration = 0.5f;
        [SerializeField, Min(0f)] private float speed = 3f;
        
        public float Distance => distance;
        public float JumpHeight => jumpHeight;
        public float Duration => duration;
        public float Speed => speed;

        public override IEnemyAttack CreateRuntime()
            => new DashAttack(this);
    }
}