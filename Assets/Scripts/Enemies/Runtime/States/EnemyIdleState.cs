using Incision.Enemies.Components;
using Incision.Enemies.Interfaces;
using UnityEngine;

namespace Incision.Enemies.Runtime.States
{
    public class EnemyIdleState : EnemyStateBase
    {
        private float _timer;
        private readonly EnemyMotor _motor;
        private readonly ITargetProvider targetProvider;

        public EnemyIdleState(EnemyBrain brain, Enemy enemy, ITargetProvider targetProvider) : base(brain, enemy)
        {
            _motor = enemy.GetComponent<EnemyMotor>();
            this.targetProvider = targetProvider;
        }

        public override void Start()
        {
            _motor.Stop();
            _timer = Brain.Profile.IdleTime;
        }

        public override void Tick()
        {
            var target = targetProvider.GetTarget(Enemy);
            if (target != null)
            {
                Brain.SetTarget(target);
                Brain.ChangeState<EnemyChaseState>();
                return;
            }

            _timer -= Time.deltaTime;
            if (_timer <= 0f)
                Brain.ChangeState<EnemyPatrolState>();
        }
    }
}