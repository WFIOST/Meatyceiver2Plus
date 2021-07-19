using FistVR;
using HarmonyLib;

namespace Meatyceiver2Plus.Failures.Firearm
{
    public static partial class FirearmFailures
    {
        [HarmonyPatch(typeof(Handgun), "CockHammer")]
        [HarmonyPrefix]
        public static bool HFHandgun(bool isManual)
        {
            if (!ConfigEntries.EnableBrokenFirearmFailures) return true;
            float rand = Common.GetRandomValue(0, 10001) / 100;
            float chance = ConfigEntries.HFRate * ConfigEntries.GeneralMultiplier;
            return !(rand <= chance) || isManual;
        }
    }
}