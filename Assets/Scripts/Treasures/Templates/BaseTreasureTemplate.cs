using Incision.Treasures.Enums;
using UnityEngine;

namespace Incision.Treasures
{
    [CreateAssetMenu(fileName = "Base Treasure Template", menuName = "General/Treasures/Base Template")]
    public class BaseTreasureTemplate : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private TreasureType type = TreasureType.GoldBag;
        [SerializeField] private string id = "gold_bag";

        [Header("Amount")]
        [SerializeField] private int amount = 1;

        [Header("Prefab")]
        [SerializeField] private Treasure treasurePrefab;
        
        public TreasureType Type => type;
        public string ID => id;
        public int Amount => amount;
        public Treasure TreasurePrefab => treasurePrefab;
    }
}
