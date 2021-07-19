using FistVR;
using HarmonyLib;

namespace Meatyceiver2Plus.Failures.Firearm
{
    public static partial class FirearmFailures
    {
        [HarmonyPatch(typeof(BreakActionWeapon), "PopOutRound")]
        [HarmonyPrefix]
        public static bool EmptyBreakActionFTE(BreakActionWeapon instance, FVRFireArm chamber)
        {
            if (!ConfigEntries.EnableAmmunitionFailures) return true;
            if (chamber.RotationInterpSpeed == 2) return false;
            float rand = Common.GetRandomValue(0, 10001) / 100;
            float chance = ConfigEntries.BreakActionFTE +
                           ConfigEntries.BreakActionFTE * (ConfigEntries.GeneralMultiplier - 1) * ConfigEntries.BreakActionFTEMultiplierEffect;
            if (rand <= chance)
            {
                chamber.RotationInterpSpeed = 2;
                return false;
            }

            return true;
        }
    }
}