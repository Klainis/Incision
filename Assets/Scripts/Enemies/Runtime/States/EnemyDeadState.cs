using Incision.Enemies.Components;
using Incision.Enemies.Interfaces;
using UnityEngine;

namespace Incision.Enemies.Runtime.States
{
    public class EnemyDeadState : EnemyStateBase
    {
        private readonly EnemyMotor _motor;
        private readonly EnemyView _view;
        private readonly Collider _collider;

        public EnemyDeadState(EnemyBrain brain, Enemy enemy) : base(brain, enemy)
        {
            _motor = enemy.GetComponent<EnemyMotor>();
            _view = enemy.GetComponent<EnemyView>();
            _collider = enemy.GetComponent<Collider>();
        }

        public override void Start()
        {
            _motor?.Stop();
            _view?.TriggerDeath();

            if (_collider != null)
                _collider.enabled = false;
        }

        public override void Tick() { }

        public override void Dispose() { }
    }
}