using FistVR;
using HarmonyLib;

namespace Meatyceiver2Plus.Failures.Firearm
{
    public static partial class FirearmFailures
    {
        [HarmonyPatch(typeof(Handgun), "EngageSlideRelease")]
        [HarmonyPrefix]
        public static bool FTLHandgunSlide()
        {
            if (!ConfigEntries.EnableFirearmFailures) return true;
            float rand = Common.GetRandomValue(0, 10001) / 100;
            float chance = ConfigEntries.FTLSlide * ConfigEntries.GeneralMultiplier;
            return !(rand <= chance);
        }
    }
}