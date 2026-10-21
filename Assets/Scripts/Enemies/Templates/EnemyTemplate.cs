using System.Collections.Generic;
using Incision.Enemies.Components;
using Incision.Enemies.Enums;
using Incision.Templates;
using UnityEngine;

namespace Incision.Enemies.Templates
{
    [CreateAssetMenu(menuName = "General/Enemy/Configuration", fileName = "EnemyTemplate")]
    public class EnemyTemplate : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private EnemyType _type;
        [SerializeField] private Enemy _prefab;
        [SerializeField] private StatsTemplate _stats;
        [SerializeField] private EnemyAITemplate _aiProfile;
        [SerializeField] private List<AttackTemplate> _attacks = new();

        public string Id => _id;
        public EnemyType Type => _type;
        public Enemy Prefab => _prefab;
        public StatsTemplate Stats => _stats;
        public EnemyAITemplate AIProfile => _aiProfile;
        public IReadOnlyList<AttackTemplate> Attacks => _attacks;
    }
}
