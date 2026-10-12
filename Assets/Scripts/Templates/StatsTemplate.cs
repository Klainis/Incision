using Incision.Enemies.Runtime;
using UnityEngine;

namespace Incision.Templates
{
    [CreateAssetMenu(menuName = "Enemies/Stats", fileName = "StatsTemplate")]
    public class StatsTemplate : ScriptableObject
    {
        [Header("Survivability")]
        [SerializeField, Range(50f, 500f)] private float _maxHealth = 100f;
        [SerializeField, Range(0f, 50f)] private float _armor = 0f;
        
        [Header("Movement")]
        [SerializeField, Range(1f, 50f)] private float _moveSpeed = 3.5f;
        [ReadOnly] [SerializeField] private float _accelerationRate = 10f;
        [ReadOnly] [SerializeField, Range(10f, 500f)] private float _acceleration;
        [SerializeField, Range(5f, 20f)] private float _turnSpeed = 10f;
        
        [Header("Detection")]
        [SerializeField, Range(0f, 50f)] private float _detectionRadius = 12;
        [SerializeField, Range(0f, 10f)] private float _loseDetectionRadiusDelta = 3;
        [ReadOnly] [SerializeField, Range(0f, 100f)] private float _loseTargetRadius;

        private void OnValidate()
        {
            _acceleration = _moveSpeed * _accelerationRate;
            _loseTargetRadius = _detectionRadius + _loseDetectionRadiusDelta;
            _loseTargetRadius = Mathf.Clamp(_loseTargetRadius, 0, _loseTargetRadius);
        }

        public float MaxHealth => _maxHealth;
        public float MoveSpeed => _moveSpeed;
        public float TurnSpeed => _turnSpeed;
        public float Armor => _armor;
        public float Acceleration => _acceleration;
        public float DetectionRadius => _detectionRadius;
        public float LoseTargetRadius => _loseTargetRadius;

        public Stats CreateRuntime() => new(this);
    }
}
