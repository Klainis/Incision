using UnityEngine;

namespace Incision
{
    [CreateAssetMenu(fileName = "AttractedPickupEffect", menuName = "Scriptable Objects/Objects/Attracted Pickup Effect")]
    public sealed class AttractedPickupEffectTemplate : ScriptableObject
    {
        [Header("Attraction")]
        [SerializeField] private float _attractDistance = 8f;
        [SerializeField] private float _pickupDistance = 0.8f;
        [SerializeField] private float _startSpeed = 30f;
        [SerializeField] private float _acceleration = 50f;
        [SerializeField] private float _maxSpeed = 80f;
        [SerializeField] private float _activationDelay = 0.25f;
        [SerializeField] private Vector3 _targetOffset = Vector3.zero;

        [Header("Lifetime")]
        [SerializeField] private float _lifeTime = 30f;

        [Header("Drop")]
        [SerializeField] private Vector2 _dropHorizontalSpeed = new Vector2(4f, 6f);
        [SerializeField] private Vector2 _dropUpSpeed = new Vector2(8f, 16f);
        [SerializeField] private float _dropGravity = 50f;
        [SerializeField, Range(0f, 1f)] private float _bounciness = 0.45f;
        [SerializeField, Range(0f, 1f)] private float _groundFriction = 0.7f;
        [SerializeField] private int _maxBounces = 2;
        [SerializeField] private float _minBounceSpeed = 2f;
        [SerializeField] private float _collisionRadius = 0.3f;
        [SerializeField] private LayerMask _groundMask = 1025;

        [Header("Idle")]
        [SerializeField] private float _spinSpeed = 180f;
        [SerializeField] private float _bobAmplitude = 0f;
        [SerializeField] private float _bobFrequency = 1.5f;

        public float AttractDistance => _attractDistance;
        public float PickupDistance => _pickupDistance;
        public float StartSpeed => _startSpeed;
        public float Acceleration => _acceleration;
        public float MaxSpeed => _maxSpeed;
        public float ActivationDelay => _activationDelay;
        public Vector3 TargetOffset => _targetOffset;
        public float LifeTime => _lifeTime;
        public Vector2 DropHorizontalSpeed => _dropHorizontalSpeed;
        public Vector2 DropUpSpeed => _dropUpSpeed;
        public float DropGravity => _dropGravity;
        public float Bounciness => _bounciness;
        public float GroundFriction => _groundFriction;
        public int MaxBounces => _maxBounces;
        public float MinBounceSpeed => _minBounceSpeed;
        public float CollisionRadius => _collisionRadius;
        public LayerMask GroundMask => _groundMask;
        public float SpinSpeed => _spinSpeed;
        public float BobAmplitude => _bobAmplitude;
        public float BobFrequency => _bobFrequency;
    }
}
