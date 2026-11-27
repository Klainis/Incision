using UnityEngine;

namespace Incision
{
    [CreateAssetMenu(fileName = "HealOrbData", menuName = "Scriptable Objects/Objects/HealOrbData")]
    public class HealOrbTemplate : ScriptableObject
    {
        [Header("Object ID")]
        [SerializeField] private string _id;

        [Header("Heal")]
        [SerializeField] private float _healAmount = 10f;
        [SerializeField] private int _orbAmount = 5;

        public string ID => _id;
        public float HealAmount => _healAmount;
        public int OrbAmount => _orbAmount;
    }
}
