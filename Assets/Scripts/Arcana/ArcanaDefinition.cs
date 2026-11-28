using UnityEngine;

namespace Incision.Arcana
{
    [CreateAssetMenu(menuName = "General/Arcana/Definition", fileName = "ArcanaDefinition")]
    public sealed class ArcanaDefinition : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private string _displayName;
        [SerializeField, TextArea] private string _description;
        [SerializeField] private ArcanaRarity _rarity;
        [SerializeField] private ArcanaEffectTemplate _effect;

        public string Id => _id;
        public string DisplayName => _displayName;
        public string Description => _description;
        public ArcanaRarity Rarity => _rarity;
        public ArcanaEffectTemplate Effect => _effect;

        public string GetBonusDescription(int level, int? nextLevel = null) =>
            _effect != null ? _effect.GetBonusDescription(level, nextLevel) : string.Empty;
    }
}
