using Incision.Treasures;
using UnityEngine;

namespace Incision.Room.Templates
{
    [CreateAssetMenu(fileName = "RoomTemplate", menuName = "General/Rooms/RoomTemplate")]
    public class RoomTemplate : ScriptableObject
    {
        [Header("General")]
        [SerializeField] [Scene] private string _sceneName;
        [SerializeField] private RoomType _type;
        [SerializeField] [Range(0.01f, 5.0f)] private int _weight = 1;

        [Header("Portals")]
        [SerializeField] private Sprite _portalIcon;
        [SerializeField] private int _maxPortals = 4;

        [Header("Treasures")]
        [SerializeField] private BaseTreasureTemplate[] _possibleTreasures;
        
        public string SceneName => _sceneName;
        public RoomType Type => _type;
        public int Weight => _weight;
        public Sprite PortalIcon => _portalIcon;
        public int MaxPortals => _maxPortals;
        public BaseTreasureTemplate Treasure => _possibleTreasures != null && _possibleTreasures.Length > 0
            ? _possibleTreasures[Random.Range(0, _possibleTreasures.Length)]
            : null;
    }
}
