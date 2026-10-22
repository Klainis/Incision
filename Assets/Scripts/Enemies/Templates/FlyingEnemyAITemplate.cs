using UnityEngine;

namespace Incision.Enemies.Templates
{
    [CreateAssetMenu(menuName = "General/Enemy/AI Profile/Flying", fileName = "Flying AI Profile")]
    public class FlyingEnemyAITemplate : EnemyAITemplate
    {
        [Header("Altitude")]
        [SerializeField] private float _minAltitude = 1.5f;
        [SerializeField] private float _maxAltitude = 12f;
        [SerializeField] private LayerMask _groundMask;

        [Header("Hover")]
        [SerializeField] private float _hoverAmplitude = 0.15f;
        [SerializeField] private float _hoverFrequency = 1.5f;

        [Header("Banking")]
        [SerializeField] private float _maxBankAngle = 35f;
        [SerializeField] private float _bankSmoothing = 6f;

        [Header("Avoidance")]
        [SerializeField] private float _avoidanceRadius = 1.2f;
        [SerializeField] private float _avoidanceLookAhead = 2.5f;
        [SerializeField] private float _avoidanceStrength = 3f;
        [SerializeField] private LayerMask _obstacleMask;

        [Header("Steering")]
        [SerializeField] private float _arriveSlowdownRadius = 3f;
        [SerializeField] private float _stopDistance = 1.2f;
        
        [Header("Strafing")]
        [SerializeField] private bool _useStrafe = false;
        [SerializeField] private float _flyingCombatHeight = 3f;
        [SerializeField] private float _strafeRadius = 6f;
        [SerializeField] private float _strafeSwitchInterval = 2f;

        public float FlyingCombatHeight => _flyingCombatHeight;
        public float StrafeRadius => _strafeRadius;
        public float StrafeSwitchInterval => _strafeSwitchInterval;
        public bool UseStrafe => _useStrafe;
        public float MinAltitude => _minAltitude;
        public float MaxAltitude => _maxAltitude;
        public LayerMask GroundMask => _groundMask;
        public float HoverAmplitude => _hoverAmplitude;
        public float HoverFrequency => _hoverFrequency;
        public float MaxBankAngle => _maxBankAngle;
        public float BankSmoothing => _bankSmoothing;
        public float AvoidanceRadius => _avoidanceRadius;
        public float AvoidanceLookAhead => _avoidanceLookAhead;
        public float AvoidanceStrength => _avoidanceStrength;
        public LayerMask ObstacleMask => _obstacleMask;
        public float ArriveSlowdownRadius => _arriveSlowdownRadius;
        public float StopDistance => _stopDistance;
    }
}
