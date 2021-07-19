using FistVR;
using HarmonyLib;

namespace Meatyceiver2Plus.Failures.Firearm
{
    public static partial class FirearmFailures
    {
        [HarmonyPatch(typeof(ClosedBolt), "BoltEvent_ArriveAtFore")]
        [HarmonyPostfix]
        public static void SfClosedBolt(ClosedBolt instance)
        {
            if (!ConfigEntries.EnableBrokenFirearmFailures) return;
            float rand = Common.GetRandomValue(0, 10001) / 100;
            float chance = ConfigEntries.SlamfireRate * ConfigEntries.GeneralMultiplier;
            if (rand <= chance)
            {
                instance.Weapon.DropHammer();
            }
        }
    }
}