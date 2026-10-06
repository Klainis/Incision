
using System.Threading.Tasks;
using Incision.Enemies.Components;
using UnityEngine;

namespace Incision.Enemies.Interfaces
{
    public interface IEnemyAttack
    {
        float Damage { get; }
        float HitRange { get; }
        float OnHitRangeMultiplier { get; }
        float Cooldown { get; }
        bool CanAttack(Transform attackPoint, Transform target);
        Task Execute(Enemy enemy, Transform attackPoint, Transform target);
    }
}