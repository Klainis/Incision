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

        public string ID => _id;
        public float HealAmount => _healAmount;
        public float AttributeactDistance => _attractDistance;
        public float PickupDistance => _pickupDistance;
        public float StartSpeed => _startSpeed;
        public float Acceleration => _acceleration;
        public float MaxSpeed => _maxSpeed;
        public float ActivationDelay => _activationDelay;
        public Vector3 TargetOffset => _targetOffset;
        public float LifeTime => _lifeTime;
    }
}
