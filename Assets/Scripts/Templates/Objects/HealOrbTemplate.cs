using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Incision
{
    [CreateAssetMenu(fileName = "HealOrbData", menuName = "Scriptable Objects/Objects/HealOrbData")]
    public class HealOrbTemplate : ScriptableObject
    {
        [Header("Object ID")]
        [SerializeField] private string _id;

        [Header("Heal")]
        [SerializeField] private float _healAmount = 10f;
        [SerializeField] private int _orbAmount = 5;

        [Header("Attraction")]
        [SerializeField] private float _attractDistance = 8f;
        [SerializeField] private float _pickupDistance = 0.6f;
        [SerializeField] private float _startSpeed = 4f;
        [SerializeField] private float _acceleration = 40f;
        [SerializeField] private float _maxSpeed = 40f;
        [SerializeField] private float _activationDelay = 0.4f;
        [SerializeField] private Vector3 _targetOffset = new Vector3(0f, 0f, 0f);

        [Header("Lifetime")]
        [SerializeField] private float _lifeTime = 30f;

        [Header("Drop (Doom Eternal style)")]
        [SerializeField] private Vector2 _dropHorizontalSpeed = new Vector2(3f, 8f); // min, max
        [SerializeField] private Vector2 _dropUpSpeed = new Vector2(8f, 14f);        // min, max
        [SerializeField] private float _dropGravity = 30f;
        [SerializeField, Range(0f, 1f)] private float _bounciness = 0.45f;
        [SerializeField, Range(0f, 1f)] private float _groundFriction = 0.6f;
        [SerializeField] private int _maxBounces = 3;
        [SerializeField] private float _minBounceSpeed = 2f;
        [SerializeField] private float _collisionRadius = 0.25f;
        [SerializeField] private LayerMask _groundMask = ~0;

        [Header("Idle")]
        [SerializeField] private float _spinSpeed = 180f;
        [SerializeField] private float _bobAmplitude = 0.15f;
        [SerializeField] private float _bobFrequency = 1.5f;

        public string ID => _id;
        public float HealAmount => _healAmount;
        public int OrbAmount => _orbAmount;
        public float AttributeactDistance => _attractDistance;
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
