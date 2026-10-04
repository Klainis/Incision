using Incision.Enemies.Components;
using UnityEngine;

namespace Incision.Enemies.Interfaces
{
    public interface ITargetProvider
    {
        Transform GetTarget(Enemy enemy);
    }
}