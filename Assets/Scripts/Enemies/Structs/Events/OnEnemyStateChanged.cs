using Incision.Enemies.Components;
using Incision.Enemies.Runtime.States;
using Incision.Extensions;
using Incision.Interfaces;
using UnityEngine;

namespace Incision.Enemies.Structs.Events
{
    public readonly struct OnEnemyStateChanged : IEvent
    {
        public readonly Enemy Enemy;
        public readonly EnemyStateBase PreviousState;
        public readonly EnemyStateBase CurrentState;
        public string Message { get; }
        
        public OnEnemyStateChanged(Enemy enemy, EnemyStateBase previousState, EnemyStateBase currentState)
        {
            Enemy = enemy;
            PreviousState = previousState;
            CurrentState = currentState;

            Message = $"{Enemy.name.AsTag(Color.red)} is now at {CurrentState.GetType().Name.AsTag(Color.cyan)} state";
        }

        
    }
}
