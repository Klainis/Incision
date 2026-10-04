using System.Collections.Generic;
using Incision.Enemies.Components;
using UnityEngine;

namespace Incision.Enemies.Templates
{
    [CreateAssetMenu(menuName = "Enemies/Enemy", fileName = "EnemyTemplate")]
    public class EnemyTemplate : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private Enemy _prefab;
        [SerializeField] private EnemyStatsTemplate _stats;
        [SerializeField] private EnemyAITemplate _aiProfile;
        [SerializeField] private List<AttackTemplate> _attacks = new();

        public string Id => _id;
        public Enemy Prefab => _prefab;
        public EnemyStatsTemplate Stats => _stats;
        public EnemyAITemplate AIProfile => _aiProfile;
        public IReadOnlyList<AttackTemplate> Attacks => _attacks;
    }
}
