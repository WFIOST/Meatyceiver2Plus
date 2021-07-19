using FistVR;
using HarmonyLib;

namespace Meatyceiver2Plus.Failures.Ammo
{
    public static partial class AmmoFailures
    {
        [HarmonyPatch(typeof(Revolver), "Fire")]
        [HarmonyPrefix]
        public static bool RevolverLPS(Revolver instance)
        {
            if (!ConfigEntries.EnableAmmunitionFailures) return true;
            float rand = Common.GetRandomValue(0, 10001) / 100;
            float chance = ConfigEntries.LPSFailureRate * ConfigEntries.GeneralMultiplier;
            if (rand <= chance)
            {
                instance.Chambers[instance.CurChamber].IsSpent = false;
                instance.Chambers[instance.CurChamber].UpdateProxyDisplay();
                return false;
            }

            return true;
        }
    }
}