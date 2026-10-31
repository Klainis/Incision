using UnityEngine;

namespace Incision.Interfaces
{
    public interface ICreature
    {
        public string Name { get; }
        public Transform Transform { get; }
    }
}
