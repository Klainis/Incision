using Incision.Enemies.Components;
using UnityEngine;

namespace Incision.Enemies.Runtime.States
{
    public class EnemyAttackState : EnemyStateBase
    {
        private readonly EnemyMotor _motor;
        private readonly EnemyAttacker _attacker;
        
        private Transform target;

        public EnemyAttackState(EnemyBrain brain, Enemy enemy) : base(brain, enemy)
        {
            _motor = enemy.GetComponent<EnemyMotor>();
            _attacker = enemy.GetComponent<EnemyAttacker>();
        }

        public override void Start()
        {
            _motor.Stop();
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

            _motor.FaceTarget(target.position);

            if (!_attacker.IsInRange(target))
            {
                Brain.ChangeState<EnemyChaseState>();
                return;
            }
            
            _attacker.TryAttack(target);
        }
    }
}