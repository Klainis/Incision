using System;
using System.Collections.Generic;
using System.Linq;

namespace Incision.Arcana
{
    public sealed class ArcanaCatalog
    {
        private readonly Random _random = new Random();
        private readonly int[] _weights = { 60, 30, 10 };
        private readonly ArcanaDefinition[] _definitions;

        public ArcanaCatalog(ArcanaDefinition[] definitions)
        {
            _definitions = (definitions ?? Array.Empty<ArcanaDefinition>())
                .Where(definition => definition != null && definition.Effect != null &&
                    !string.IsNullOrWhiteSpace(definition.Id))
                .Where(definition => Enum.IsDefined(typeof(ArcanaRarity), definition.Rarity))
                .GroupBy(definition => definition.Id, StringComparer.Ordinal)
                .Select(group => group.First())
                .ToArray();
        }

        public IReadOnlyList<ArcanaDefinition> DrawOffers(int count)
        {
            var pools = _definitions.GroupBy(definition => definition.Rarity)
                .ToDictionary(group => group.Key, group => group.ToList());
            var offers = new List<ArcanaDefinition>();
            while (offers.Count < count && pools.Count > 0)
            {
                int roll = _random.Next(pools.Keys.Sum(rarity => _weights[(int)rarity]));
                foreach (var rarity in pools.Keys.OrderBy(rarity => rarity).ToArray())
                {
                    roll -= _weights[(int)rarity];
                    if (roll >= 0)
                        continue;

                    var pool = pools[rarity];
                    int index = _random.Next(pool.Count);
                    offers.Add(pool[index]);
                    pool.RemoveAt(index);
                    if (pool.Count == 0)
                        pools.Remove(rarity);
                    break;
                }
            }

            return offers.AsReadOnly();
        }
    }
}
