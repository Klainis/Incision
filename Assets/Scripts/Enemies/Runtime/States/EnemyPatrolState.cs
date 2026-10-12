using Incision.Enemies.Components;
using Incision.Enemies.Interfaces;
using UnityEngine;

namespace Incision.Enemies.Runtime.States
{
    public class EnemyPatrolState : EnemyStateBase
    {
        private readonly EnemyMotor _motor;
        private Vector3 _home;
        private readonly ITargetProvider _targetProvider;
        
        private Transform target;
        private Vector3 destination;

        public EnemyPatrolState(EnemyBrain brain, Enemy enemy, ITargetProvider targetProvider) : base(brain, enemy)
        {
            _motor = enemy.Motor;
            _targetProvider = targetProvider;
        }

        public override void Start()
        {
            _home = Enemy.transform.position;
            PickNewDestination();
        }

        public override void Tick()
        {
            target = _targetProvider.GetTarget(Enemy.transform, Enemy.Stats.DetectionRadius);
            if (target != null)
            {
                Brain.SetTarget(target);
                Brain.ChangeState<EnemyChaseState>();
                return;
            }
            
            _motor.FaceTarget(destination);
            if (_motor.ReachedDestination)
                PickNewDestination();
        }

        private void PickNewDestination()
        {
            Vector2 rnd = Random.insideUnitCircle * Brain.Profile.PatrolRadius;
            destination = _home + new Vector3(rnd.x, 0f, rnd.y);
            _motor.MoveTo(destination);
        }
    }
}