using System.Globalization;
using UnityEngine;

namespace Incision.Arcana
{
    [CreateAssetMenu(menuName = "General/Arcana/Effect", fileName = "ArcanaEffect")]
    public sealed class ArcanaEffectTemplate : ScriptableObject
    {
        [SerializeField] private ArcanaEffect _kind;
        [SerializeField, Min(0f)] private float _strengthPerLevel;
        [SerializeField, Min(0f)] private float _duration;
        [SerializeField] private string _bonusPrefix;
        [SerializeField] private string _bonusSuffix;

        public ArcanaEffect Kind => _kind;
        public float Duration => _duration;

        public float GetStrengthAtLevel(int level) => _strengthPerLevel * level;

        public string GetBonusDescription(int level, int? nextLevel = null)
        {
            string bonus = FormatBonus(level);
            if (nextLevel.HasValue)
                bonus += " → " + FormatBonus(nextLevel.Value);

            if (!string.IsNullOrWhiteSpace(_bonusPrefix))
                bonus = _bonusPrefix.Trim() + " " + bonus;
            if (!string.IsNullOrWhiteSpace(_bonusSuffix))
                bonus += " " + _bonusSuffix.Trim();
            if (_duration > 0f)
                bonus += " for " + _duration.ToString("0.##", CultureInfo.InvariantCulture) + " s";

            return bonus;
        }

        private string FormatBonus(int level)
        {
            float percentage = GetStrengthAtLevel(level) * 100f;
            return "+" + percentage.ToString("0.##", CultureInfo.InvariantCulture) + "%";
        }
    }
}
