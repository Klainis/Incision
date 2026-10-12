using System.Collections.Generic;
using Incision.Enemies.Runtime;
using UnityEngine;

namespace Incision.Enemies.Templates
{
    [CreateAssetMenu(menuName = "Enemies/Patterns/Wave", fileName = "WavePatternTemplate")]
    public class WavePatternTemplate : ScriptableObject
    {
        [SerializeField] private List<Wave> _waves = new();
        public IReadOnlyList<Wave> Waves => _waves;
    }
}
