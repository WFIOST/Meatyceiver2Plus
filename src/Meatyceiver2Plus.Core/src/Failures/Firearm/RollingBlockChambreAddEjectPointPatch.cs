using FistVR;
using HarmonyLib;

namespace Meatyceiver2Plus.Failures.Firearm
{
    public static partial class FirearmFailures
    {
        [HarmonyPatch(typeof(FVRFireArmChamber), "Awake")]
        [HarmonyPrefix]
        public static bool RollingBlockChamberAddEjectPointPatch(FVRFireArmChamber instance)
        {
            if (instance.Firearm is RollingBlock) instance.IsManuallyExtractable = true;
            return true;
        }
    }
}