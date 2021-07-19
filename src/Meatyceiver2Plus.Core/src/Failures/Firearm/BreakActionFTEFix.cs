using FistVR;
using HarmonyLib;

namespace Meatyceiver2Plus.Failures.Firearm
{
    public static partial class FirearmFailures
    {
        [HarmonyPatch(typeof(FVRFireArmChamber), "BeginInteraction")]
        [HarmonyPostfix]
        public static void BreakActionFTEFix(FVRFireArmChamber instance) => instance.RotationInterpSpeed = 1;
    }
}