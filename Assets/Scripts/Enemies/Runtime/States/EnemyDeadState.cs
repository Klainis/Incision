using Incision.Enemies.Components;
using Incision.Enemies.Interfaces;
using UnityEngine;

namespace Incision.Enemies.Runtime.States
{
    public class EnemyDeadState : EnemyStateBase
    {
        private readonly EnemyMotor _motor;
        private readonly EnemyView _view;

        public EnemyDeadState(EnemyBrain brain, Enemy enemy) : base(brain, enemy)
        {
            _motor = enemy.Motor;
            _view = enemy.View;
        }

        public override void Start()
        {
            _motor?.Stop();
            _view?.TriggerDeath();
            Object.Destroy(Enemy.gameObject);
        }

        public override void Tick() { }

        public override void Dispose() { }
    }
}