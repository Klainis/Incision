using System;
using Incision.Enemies.Components;
using VContainer.Unity;

namespace Incision.Enemies.Runtime.States
{
    public abstract class EnemyStateBase : IStartable, ITickable, IDisposable
    {
        protected readonly EnemyBrain Brain;
        protected readonly Enemy Enemy;

        protected EnemyStateBase(EnemyBrain brain, Enemy enemy)
        {
            Brain = brain;
            Enemy = enemy;
        }
        
        public virtual void Start() {}

        public virtual void Tick() {}

        public virtual void Dispose() {}
    }
}