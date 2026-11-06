using Incision.Room.Templates;
using UnityEngine;

namespace Incision
{
    [CreateAssetMenu(fileName = "RoomGraphTemplate", menuName = "General/Rooms/Graph/Settings")]
    public class RoomGraphTemplate : ScriptableObject
    {
        [SerializeField] private RoomCatalog roomCatalog;
        [SerializeField, Range(1, 5000)] private int maxSpawnAttempts = 2500;
        
        public RoomCatalog Catalog => roomCatalog;
        public int MaxSpawnAttempts => maxSpawnAttempts;
    }
}
