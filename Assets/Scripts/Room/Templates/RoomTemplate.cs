using Incision.Treasures;
using UnityEngine;

namespace Incision.Room.Templates
{
    [CreateAssetMenu(fileName = "RoomTemplate", menuName = "General/Rooms/RoomTemplate")]
    public class RoomTemplate : ScriptableObject
    {
        [SerializeField] [Scene] private string sceneName;
        [SerializeField] private RoomType type;
        [SerializeField] [Range(0.01f, 5.0f)] private int weight = 1;
        [SerializeField] private int maxPortals = 4;
        [SerializeField] private BaseTreasureTemplate[] possibleTreasures;
        
        public string SceneName => sceneName;
        public RoomType Type => type;
        public int Weight => weight;
        public int MaxPortals => maxPortals;
        public BaseTreasureTemplate Treasure => possibleTreasures[Random.Range(0, possibleTreasures.Length)];
    }
}
