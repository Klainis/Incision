using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Incision.Room.Templates
{
    [CreateAssetMenu(fileName = "RoomCatalog", menuName = "General/Rooms/Catalog")]
    public class RoomCatalog : ScriptableObject
    {
        [SerializeField] private List<RoomTemplate> rooms = new List<RoomTemplate>();
        
        public IReadOnlyList<RoomTemplate> Rooms => rooms;

        private Dictionary<RoomType, int> _weights = new Dictionary<RoomType, int>();

        public RoomTemplate GetRandomRoom(RoomType type = RoomType.NotSet)
        {   
            var valid = Rooms.Where(rt => rt.Type == type);
            var weight = _weights[type];
            
            if (type != RoomType.NotSet)
                return Weighted(valid.ToList(), weight);
            
            valid = Rooms.Where(rt => rt.Type != RoomType.Start && rt.Type != RoomType.NotSet);
            weight = Rooms.Sum(r => r.Weight) - GetWeights(RoomType.Start);
            

            return Weighted(valid.ToList(), weight);
        }

        private void OnEnable()
        {
            _weights.Clear();
            _weights.Add(RoomType.Start, GetWeights(RoomType.Start));
            _weights.Add(RoomType.Shop, GetWeights(RoomType.Shop));
            _weights.Add(RoomType.Boss, GetWeights(RoomType.Boss));
            _weights.Add(RoomType.Normal, GetWeights(RoomType.Normal));
            _weights.Add(RoomType.Treasure, GetWeights(RoomType.Treasure));
            _weights.Add(RoomType.NotSet, 0);
        }

        private int GetWeights(RoomType type) => Rooms.Where(r => r.Type == type).Sum(r => r.Weight);

        public RoomTemplate StartRoom => GetRandomRoom(RoomType.Start);
        public RoomTemplate EndRoom => GetRandomRoom(RoomType.Boss);
        
        private static RoomTemplate Weighted(IReadOnlyList<RoomTemplate> list, int listWeight)
        {
            var roll  = Random.Range(0, listWeight);
            foreach (var r in list)
            {
                if (roll < r.Weight) 
                    return r; 
                roll -= r.Weight;
            }
            return list[0];
        }
    }
}
