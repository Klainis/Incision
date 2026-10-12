using UnityEngine;

namespace Incision.Enemies.Components
{
    public class EnemyView : MonoBehaviour
    {
        private Animator _animator;
        // [SerializeField] private string _speedParam = "Speed";
        [SerializeField] private string _deathTrigger = "Death";
        
        private EnemyMotor _motor;

        public void Initialize(EnemyMotor enemyMotor, Animator animator)
        {
            _motor = enemyMotor;
            _animator = animator;
        }

        // private void Update()
        // {
        //     if (_animator == null || _motor == null) return;
        //
        //     float speed = _motor.ReachedDestination ? 0f : 1f;
        //     _animator.SetFloat(_speedParam, speed, 0.1f, Time.deltaTime);
        // }

        public void SetSwitch(string switchParam, bool switchValue)
        {
            if (_animator == null || string.IsNullOrEmpty(switchParam)) 
                return;
            
            _animator.SetBool(switchParam, switchValue);
        }

        public void TriggerEvent(string trigger)
        {
            if (_animator == null || string.IsNullOrEmpty(trigger)) 
                return;
            
            _animator.SetTrigger(trigger);
        }

        public void TriggerAttack(string trigger) => TriggerEvent(trigger);

        public void TriggerDeath() => TriggerEvent(_deathTrigger);
    }
}