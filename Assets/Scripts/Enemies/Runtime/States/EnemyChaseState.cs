using Incision.Enemies.Components;
using UnityEngine;

namespace Incision.Enemies.Runtime.States
{

    public class EnemyChaseState : EnemyStateBase
    {
        private readonly EnemyMotor _motor;
        private readonly EnemyAttacker _attacker;
        private readonly EnemyView _view;
        private Transform target;

        public EnemyChaseState(EnemyBrain brain, Enemy enemy) : base(brain, enemy)
        {
            _motor = enemy.GetComponent<EnemyMotor>();
            _attacker = enemy.GetComponent<EnemyAttacker>();
            _view = enemy.GetComponent<EnemyView>();
        }

        public override void Start()
        {
            // _view.SetSwitch("IsRunning", true);
            target = Brain.CurrentTarget;
        }

        public override void Dispose()
        {
            target = null;
            base.Dispose();
        }

        public override void Tick()
        {
            if (target == null)
            {
                Brain.ChangeState<EnemyPatrolState>();
                return;
            }

            float dist = Vector3.Distance(Enemy.transform.position, target.position);
            if (dist > Enemy.Stats.LoseTargetRadius)
            {
                Brain.SetTarget(null);
                Brain.ChangeState<EnemyPatrolState>();
                return;
            }

            if (_attacker != null && _attacker.IsInRange(target))
            {
                Brain.ChangeState<EnemyAttackState>();
                return;
            }
            
            _motor.FaceTarget(target.position);
            _motor.MoveTo(target.position);
        }
    }
}