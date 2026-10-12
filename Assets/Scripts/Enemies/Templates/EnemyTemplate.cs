using System.Collections.Generic;
using Incision.Enemies.Components;
using Incision.Templates;
using UnityEngine;

namespace Incision.Enemies.Templates
{
    [CreateAssetMenu(menuName = "General/Enemy", fileName = "EnemyTemplate")]
    public class EnemyTemplate : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private Enemy _prefab;
        [SerializeField] private StatsTemplate _stats;
        [SerializeField] private EnemyAITemplate _aiProfile;
        [SerializeField] private List<AttackTemplate> _attacks = new();

        public string Id => _id;
        public Enemy Prefab => _prefab;
        public StatsTemplate Stats => _stats;
        public EnemyAITemplate AIProfile => _aiProfile;
        public IReadOnlyList<AttackTemplate> Attacks => _attacks;
    }
}
