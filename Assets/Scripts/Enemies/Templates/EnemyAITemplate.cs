using UnityEngine;

namespace Incision.Enemies.Templates
{
    [CreateAssetMenu(menuName = "Enemies/AI Profile", fileName = "AI Profile")]
    public class EnemyAITemplate : ScriptableObject
    {
        [SerializeField] private float _idleTime = 2f;
        [SerializeField] private float _patrolRadius = 5f;
        [SerializeField] private float _chaseSpeedMultiplier = 1f;
        [SerializeField] private float _attackSpeedMultiplier = 1f;

        public float IdleTime => _idleTime;
        public float PatrolRadius => _patrolRadius;
        public float ChaseSpeedMultiplier => _chaseSpeedMultiplier;
        public float AttackSpeedMultiplier => _attackSpeedMultiplier;
    }
}
