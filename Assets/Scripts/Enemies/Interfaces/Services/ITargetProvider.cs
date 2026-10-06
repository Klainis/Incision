using UnityEngine;

namespace Incision.Enemies.Interfaces
{
    public interface ITargetProvider
    {
        Transform GetTarget(Transform transform, float range);
    }
}