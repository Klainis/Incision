using Incision.Enemies.Enums;
using UnityEngine;

namespace Incision.Enemies.Templates
{
    [CreateAssetMenu(menuName = "Enemies/AI Profile", fileName = "AI Profile")]
    public class EnemyAITemplate : ScriptableObject
    {
        [SerializeField] private EnemyType _type;
        [SerializeField] private float _idleTime = 2f;
        [SerializeField] private float _patrolRadius = 5f;

        public EnemyType Type => _type;
        public float IdleTime => _idleTime;
        public float PatrolRadius => _patrolRadius;
    }
}
