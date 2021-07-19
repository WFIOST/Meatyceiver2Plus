using FistVR;
using HarmonyLib;

using Meatyceiver2Plus;

namespace Meatyceiver2Plus.Failures.Ammo
{
    public static partial class AmmoFailures
    {
        [HarmonyPatch(typeof(FVRFireArmChamber), "Fire")]
        [HarmonyPrefix]
        private static bool LightPrimerStrike(ref bool result, FVRFireArmChamber instance, FVRFireArmRound mRound)
        {
            if (!ConfigEntries.EnableAmmunitionFailures) return true;
            if (instance.Firearm is Revolver or RevolvingShotgun) return true;
            float rand = Common.GetRandomValue(0, 10001) / 100;
            float chance = ConfigEntries.LPSFailureRate * ConfigEntries.GeneralMultiplier;
            if (rand >= chance)
            {
                if (instance.IsFull && mRound != null && !instance.IsSpent)
                {
                    instance.IsSpent = true;
                    instance.UpdateProxyDisplay();
                    result = true;
                    return false;
                }
            }

            result = false;
            return false;
        }
    }
}