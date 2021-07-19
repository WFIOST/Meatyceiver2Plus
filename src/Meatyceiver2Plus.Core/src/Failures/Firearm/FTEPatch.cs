using FistVR;
using HarmonyLib;

namespace Meatyceiver2Plus.Failures.Firearm
{
    public static partial class FirearmFailures
    {
        [HarmonyPatch(typeof(ClosedBolt), "ImpartFiringImpulse")]
        [HarmonyPatch(typeof(HandgunSlide), "ImpartFiringImpulse")]
        [HarmonyPatch(typeof(OpenBoltReceiverBolt), "ImpartFiringImpulse")]
        [HarmonyPrefix]
        public static bool FTEPatch(FVRInteractiveObject instance)
        {
            if (instance is BoltActionRifle or LeverActionFirearm) return false;
            if (!ConfigEntries.EnableFirearmFailures) return true;
            float rand = Common.GetRandomValue(0, 10001) / 100;
            float chance = ConfigEntries.FTERate * ConfigEntries.GeneralMultiplier;
            if (rand <= chance)
            {
                instance.RotationInterpSpeed = 2;
                return false;
            }
            return true;
        }
    }
}