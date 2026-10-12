using System;

namespace Incision.Extensions
{
    public static partial class FloatExtensions
    {
        public static int AsMilliseconds(this float var)
        {
            return Convert.ToInt32(var * 1000);
        }
    }
}
