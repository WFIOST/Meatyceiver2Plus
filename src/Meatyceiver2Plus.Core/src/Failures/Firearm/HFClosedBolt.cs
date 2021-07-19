using FistVR;
using HarmonyLib;

namespace Meatyceiver2Plus.Failures.Firearm
{
    public static partial class FirearmFailures
    {
        [HarmonyPatch(typeof(ClosedBoltWeapon), "CockHammer")]
        [HarmonyPrefix]
        public static bool HFClosedBolt()
        {
            if (!ConfigEntries.EnableFirearmFailures) return true;
            float rand = Common.GetRandomValue(0, 10001) / 100;
            float chance = ConfigEntries.HFRate * ConfigEntries.GeneralMultiplier;
            return !(rand <= chance);
        }
    }
}