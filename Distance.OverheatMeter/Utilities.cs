using UnityEngine;

namespace Distance.Overheatmeter
{
    internal static class Utilities
    {
        internal static GameObject FindLocalCar()
        {
            return G.Sys.PlayerManager_?.Current_?.playerData_?.Car_;
        }

        internal static CarLogic FindLocalCarLogic()
        {
            return G.Sys.PlayerManager_?.Current_?.playerData_?.CarLogic_;
        }

        internal static class MathUtil
        {
            public static float Map(float s, float a1, float a2, float b1, float b2)
            {
                return b1 + ((s - a1) * (b2 - b1) / (a2 - a1));
            }
        }
    }
}
