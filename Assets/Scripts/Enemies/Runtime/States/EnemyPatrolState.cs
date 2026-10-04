using Incision.Enemies.Components;
using Incision.Enemies.Interfaces;
using Unity.VisualScripting;
using UnityEngine;

namespace Incision.Enemies.Runtime.States
{
    public class EnemyPatrolState : EnemyStateBase
    {
        private readonly EnemyMotor _motor;
        private readonly EnemyView _view;
        private Vector3 _home;
        private readonly ITargetProvider _targetProvider;
        
        private Transform target;

        public EnemyPatrolState(EnemyBrain brain, Enemy enemy, ITargetProvider targetProvider) : base(brain, enemy)
        {
            _motor = enemy.GetComponent<EnemyMotor>();
            _view = enemy.GetComponent<EnemyView>();
            _targetProvider = targetProvider;
        }

        public override void Start()
        {
            
            _home = Enemy.transform.position;
            _view.SetSwitch("IsRunning", false);
            PickNewDestination();
        }

        public override void Tick()
        {
            target = _targetProvider.GetTarget(Enemy);
            if (target != null)
            {
                Brain.SetTarget(target);
                Brain.ChangeState<EnemyChaseState>();
                return;
            }

            if (_motor.ReachedDestination)
                PickNewDestination();
        }

        private void PickNewDestination()
        {
            Vector2 rnd = Random.insideUnitCircle * Brain.Profile.PatrolRadius;
            Vector3 dest = _home + new Vector3(rnd.x, 0f, rnd.y);
            _motor.MoveTo(dest);
        }
    }
}