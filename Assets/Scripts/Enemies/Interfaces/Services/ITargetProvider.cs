using Incision.Interfaces;
using UnityEngine;

namespace Incision.Enemies.Interfaces
{
    public interface ITargetProvider : IService
    {
        Transform GetTarget(Transform transform, float range);
    }
}