using FistVR;
using HarmonyLib;

namespace Meatyceiver2Plus.Failures.Firearm
{
    public static partial class FirearmFailures
    {
        [HarmonyPatch(typeof(FVRFireArmChamber), "EjectRound")]
        [HarmonyPrefix]
        public static bool RevolverRollingBlockFTE(FVRFireArmChamber instance)
        {
            if (!ConfigEntries.EnableFirearmFailures) return true;
            switch (instance.Firearm)
            {
                case Revolver:
                {
                    if (instance.RotationInterpSpeed == 1)
                    {
                        float rand = Common.GetRandomValue(0, 10001) / 100;
                        float chance = ConfigEntries.RevolverFTE +
                                       ConfigEntries.RevolverFTE * (ConfigEntries.GeneralMultiplier - 1) *
                                       ConfigEntries.RevolverFTEGeneralMultiplierEffect;
                        if (rand <= chance)
                        {
                            instance.RotationInterpSpeed = 2;
                            return false;
                        }
                    }

                    break;
                }
                case RollingBlock:
                {
                    float rand = Common.GetRandomValue(0, 10001) / 100;
                    float chance = ConfigEntries.BreakActionFTE +
                                   ConfigEntries.BreakActionFTE * (ConfigEntries.GeneralMultiplier - 1) * ConfigEntries.BreakActionFTEMultiplierEffect;
                    if (rand <= chance)
                    {
                        return false;
                    }

                    break;
                }
            }

            return true;
        }
    }
}