using UnityEngine;

namespace Incision.Enemies.Templates
{
    [CreateAssetMenu(menuName = "Enemies/AI Profile", fileName = "AI Profile")]
    public class EnemyAITemplate : ScriptableObject
    {
        [SerializeField] private float _idleTime = 2f;
        [SerializeField] private float _patrolRadius = 5f;

        public float IdleTime => _idleTime;
        public float PatrolRadius => _patrolRadius;
    }
}
