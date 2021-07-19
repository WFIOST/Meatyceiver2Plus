using FistVR;
using HarmonyLib;

namespace Meatyceiver2Plus.Failures.Firearm
{
    public static partial class FirearmFailures
    {
        [HarmonyPatch(typeof(HandgunSlide), "SlideEvent_ArriveAtFore")]
        [HarmonyPostfix]
        public static void SfHandgun(HandgunSlide instance)
        {
            if (!ConfigEntries.EnableAmmunitionFailures) return;
            string failureName = "Slam fire";
            float rand = Common.GetRandomValue(0, 10001) / 100;
            float chance = ConfigEntries.SlamfireRate * ConfigEntries.GeneralMultiplier;
            if (rand <= chance)
            {
                instance.Handgun.DropHammer(false);
            }
        }
    }
}