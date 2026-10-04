using Incision.Enemies.Runtime;
using UnityEngine;

namespace Incision.Enemies.Templates
{
    [CreateAssetMenu(menuName = "Enemies/Stats", fileName = "EnemyStatsTemplate")]
    public class EnemyStatsTemplate : ScriptableObject
    {
        [SerializeField] private float _maxHealth = 100;
        [SerializeField] private float _moveSpeed = 3.5f;
        [SerializeField] private float _turnSpeed = 10f;
        [SerializeField] private float _armor = 0;
        [SerializeField] private float _detectionRadius = 12;
        [SerializeField] private float _loseTargetRadius = 16;

        public float MaxHealth => _maxHealth;
        public float MoveSpeed => _moveSpeed;
        public float TurnSpeed => _turnSpeed;
        public float Armor => _armor;
        public float DetectionRadius => _detectionRadius;
        public float LoseTargetRadius => _loseTargetRadius;

        public EnemyStats CreateRuntime() => new(this);
    }
}
