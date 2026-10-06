using Incision.Enemies.Runtime;
using UnityEngine;

namespace Incision.Enemies.Templates
{
    [CreateAssetMenu(menuName = "Enemies/Stats", fileName = "EnemyStatsTemplate")]
    public class StatsTemplate : ScriptableObject
    {
        [SerializeField] private float _maxHealth = 100;
        [SerializeField] private float _moveSpeed = 3.5f;
        [SerializeField] private float _turnSpeed = 10f;
        [SerializeField] private float _armor = 0;
        [SerializeField, Range(0, 50)] private float _detectionRadius = 12;
        [SerializeField, Range(0, 10)] private float _loseDetectionRadiusDelta = 3;
        [ReadOnly] [SerializeField, Range(0, 100)] private float _loseTargetRadius;

        private void OnValidate()
        {
            _loseTargetRadius = _detectionRadius + _loseDetectionRadiusDelta;
            _loseTargetRadius = Mathf.Clamp(_loseTargetRadius, 0, _loseTargetRadius);
        }

        public float MaxHealth => _maxHealth;
        public float MoveSpeed => _moveSpeed;
        public float TurnSpeed => _turnSpeed;
        public float Armor => _armor;
        public float DetectionRadius => _detectionRadius;
        public float LoseTargetRadius => _loseTargetRadius;

        public Stats CreateRuntime() => new(this);
    }
}
