using Incision.Enemies.Components;
using Incision.Enemies.Templates;
using UnityEngine;

namespace Incision.Enemies.Runtime.States
{
    public class EnemyStrafeState : EnemyStateBase
    {
        private readonly EnemyMotor _motor;
        private readonly EnemyAttacker _attacker;
        private readonly FlyingEnemyAITemplate _profile;
        private float _strafeSign;
        private float _switchTimer;
        
        
        private Transform target;

        public EnemyStrafeState(EnemyBrain brain, Enemy enemy, FlyingEnemyAITemplate profile)
            : base(brain, enemy)
        {
            _motor    = enemy.Motor;
            _attacker = enemy.Attacker;
            _profile = profile;
        }

        public override void Start()
        {
            _strafeSign = Random.value > 0.5f ? 1f : -1f;
            _switchTimer = Random.Range(1f, _profile.StrafeSwitchInterval);
            target = Brain.CurrentTarget;
        }

        public override void Tick()
        {
            if (_attacker.IsAttacking)
                return;
            
            if (!target)
            {
                Brain.ChangeState<EnemyPatrolState>(); 
                return;
            }

            if (!_attacker.IsInRange(target))
            {
                Brain.ChangeState<EnemyChaseState>();
                return;
            }

            _switchTimer -= Time.deltaTime;
            if (_switchTimer <= 0f)
            {
                _strafeSign = -_strafeSign;
                _switchTimer = Random.Range(1f, _profile.StrafeSwitchInterval);
            }

            Vector3 toEnemy = Enemy.transform.position - target.position;
            Vector3 flat = new Vector3(toEnemy.x, 0f, toEnemy.z);
            if (flat.sqrMagnitude < 0.01f) flat = Enemy.transform.forward;

            Vector3 tangent = Vector3.Cross(Vector3.up, flat.normalized) * _strafeSign;

            float radius = _profile.StrafeRadius;
            Vector3 desired = target.position
                              + flat.normalized * radius
                              + tangent * radius * 0.6f
                              + Vector3.up * _profile.FlyingCombatHeight;

            _motor.MoveTo(desired);
            _motor.FaceTarget(target.position);

            if (_attacker.IsInRange(target))
                _ = _attacker.TryAttack(target);
        }
    }
}
