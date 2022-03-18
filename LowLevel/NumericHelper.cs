using System;

namespace MupenToolkitPRE.LowLevel
{
    public static class NumericHelper
    {
        public static int ClampFrame(int samples, int frame)
        {
            return Math.Clamp(frame, 0, Math.Max(0, samples - 1));
        }
    }
}
