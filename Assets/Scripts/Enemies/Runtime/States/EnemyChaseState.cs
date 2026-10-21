using Incision.Enemies.Components;
using Incision.Enemies.Enums;
using UnityEngine;

namespace Incision.Enemies.Runtime.States
{
    public class EnemyChaseState : EnemyStateBase
    {
        private readonly EnemyMotor _motor;
        private readonly EnemyAttacker _attacker;
        private readonly EnemyView _view;
        
        private Transform _target;

        public EnemyChaseState(EnemyBrain brain, Enemy enemy) : base(brain, enemy)
        {
            _motor = enemy.Motor;
            _attacker = enemy.Attacker;
            _view = enemy.View;
        }

        public override void Start()
        {
            _view.SetSwitch("IsRunning", true);
            _target = Brain.CurrentTarget;
        }

        public override void Dispose()
        {
            _view.SetSwitch("IsRunning", false);
            _target = null;
            base.Dispose();
        }

        public override void Tick()
        {
            if (!_target)
            {
                Brain.ChangeState<EnemyPatrolState>();
                return;
            }

            float dist = Vector3.Distance(Enemy.transform.position, _target.position);
            if (dist > Enemy.Stats.LoseTargetRadius)
            {
                Brain.SetTarget(null);
                Brain.ChangeState<EnemyPatrolState>();
                return;
            }

            if (_attacker && _attacker.IsInRange(_target))
            {
                if (Enemy.Template.Type == EnemyType.Flying && 
                    Brain.HasState<EnemyStrafeState>())
                    Brain.ChangeState<EnemyStrafeState>();
                else
                    Brain.ChangeState<EnemyAttackState>();
                return;
            }
            
            _motor.FaceTarget(_target.position);
            _motor.MoveTo(_target.position);
        }
    }
}