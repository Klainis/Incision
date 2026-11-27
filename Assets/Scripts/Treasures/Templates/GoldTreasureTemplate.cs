using UnityEngine;

namespace Incision.Treasures
{
    [CreateAssetMenu(fileName = "Gold Template", menuName = "General/Treasures/Gold Template")]
    public class GoldTreasureTemplate : BaseTreasureTemplate
    {
        [Header("Value")]
        [SerializeField] private int minGold = 1;
        [SerializeField] private int maxGold = 100;

        public int MinGold => minGold;
        public int MaxGold => maxGold;
    }
}
