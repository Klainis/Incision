using Incision.Room.Templates;
using UnityEngine;

namespace Incision
{
    [CreateAssetMenu(fileName = "RoomGraphTemplate", menuName = "General/Rooms/Graph/Settings")]
    public class RoomGraphTemplate : ScriptableObject
    {
        [Header("Rooms Catalog")]
        [SerializeField] private RoomCatalog _roomCatalog;
        [SerializeField, Range(1, 5000)] private int _maxSpawnAttempts = 2500;

        [Header("Rooms (min, max)")]
        [SerializeField] private Vector2Int _goldCount = new(5, 7);
        [SerializeField] private Vector2Int _altarCount = new(2, 3);
        [SerializeField] private Vector2Int _arcanaCount = new(2, 3);
        [SerializeField] private Vector2Int _eliteCount = new(1, 2);
        [SerializeField] private Vector2Int _hubCount = new(1, 2);
        [SerializeField] private Vector2Int _shopCount = new(1, 2);

        public RoomCatalog Catalog => _roomCatalog;
        public int MaxSpawnAttempts => _maxSpawnAttempts;
        public Vector2Int GoldCount => _goldCount;
        public Vector2Int AltarCount => _altarCount;
        public Vector2Int ArcanaCount => _arcanaCount;
        public Vector2Int EliteCount => _eliteCount;
        public Vector2Int HubCount => _hubCount;
        public Vector2Int ShopCount => _shopCount;
    }
}
