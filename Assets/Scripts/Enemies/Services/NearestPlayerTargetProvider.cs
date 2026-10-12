using Incision.Enemies.Interfaces;
using Incision.Extensions;
using UnityEngine;

namespace Incision.Enemies.Services
{
    public class NearestPlayerTargetProvider : ITargetProvider
    {
        public string LOG_TAG { get; } = "[Nearest Player Target Provider]".AsTag(Color.green);
        
        private readonly Transform _target;

        public NearestPlayerTargetProvider(Transform target)
        {
            _target = target;
        }

        public Transform GetTarget(Transform transform, float range)
        {
            if (_target == null) return null;
            float dist = Vector3.Distance(transform.position, _target.position);
            return dist <= range ? _target : null;
        }
    }
}