using UnityEngine;

namespace Incision.Enemies.Components
{
    public class EnemyView : MonoBehaviour
    {
        private Animator _animator;
        
        [SerializeField] private string _deathTrigger = "Death";
        
        private EnemyMotor _motor;

        public void Initialize(EnemyMotor enemyMotor, Animator animator)
        {
            _motor = enemyMotor;
            _animator = animator;
        }

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