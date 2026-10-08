using Unity.VisualScripting;
using UnityEngine;

namespace Incision.Extensions
{
    public static partial class StringExtensions
    {
        public static string AsTag(this string str, Color color) => str.AsTag(color.ToHexString());
        
        public static string AsTag(this string str, string hex)
        {
            if (hex[0] != '#')
                hex = "#" + hex;
            return $"<color={hex}>{str}</color>";
        }
        
        public static string Bold(this string str)
        {
            return $"<b>{str}</b>";
        }
    }
}
