
using Incision.Enemies.Components;
using UnityEngine;

namespace Incision.Enemies.Interfaces
{
    public interface IEnemyAttack
    {
        float Damage { get; }
        float Range { get; }
        float Cooldown { get; }
        bool CanAttack(Transform attackPoint, Transform target);
        void Execute(Enemy enemy, Transform attackPoint, Transform target);
    }
}