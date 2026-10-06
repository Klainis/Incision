
using Incision.Enemies.Interfaces;
using Incision.Enemies.Runtime;
using Incision.Enemies.Templates;
using UnityEngine;
using UnityEngine.AI;
using VContainer;

namespace Incision.Enemies.Components
{
    [RequireComponent(typeof(Health), typeof(EnemyMotor), typeof(EnemyBrain))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private EnemyMotor _motor;
        [SerializeField] private EnemyBrain _brain;
        [SerializeField] private EnemyAttacker _attacker;
        [SerializeField] private EnemyView _view;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Animator _animator;
        [SerializeField] private EnemyAnimEvents _animEvents;
        
        [Inject] private ITargetProvider _targetProvider;
        [Inject] private INavMeshService _navMesh;
        [Inject] private IEventBus _eventBus;

        private EnemyTemplate template;
        private Stats stats;

        public EnemyTemplate Template => template;
        public EnemyView View => _view;
        public EnemyMotor Motor => _motor;
        public EnemyAttacker Attacker => _attacker;
        public EnemyAnimEvents AnimationEvents => _animEvents;
        public Stats Stats => stats;

        public void Initialize(EnemyTemplate tpl)
        {
            template = tpl;
            stats = template.Stats.CreateRuntime();

            _health.Initialize(stats);
            _motor.Initialize(_navMeshAgent, stats, _navMesh);
            _brain.Initialize(this, template.AIProfile, _health, _targetProvider);
            
            if (_attacker == null)
            {
                _attacker = gameObject.AddComponent<EnemyAttacker>();
            }
            _attacker.Initialize(this, template.Attacks);

            if (_view == null)
            {
                _view = gameObject.AddComponent<EnemyView>();
            }
            _view.Initialize(_motor, _animator);
        }
    }
}
