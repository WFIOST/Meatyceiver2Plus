using FistVR;
using HarmonyLib;

namespace Meatyceiver2Plus.Failures.Firearm
{
    public static partial class FirearmFailures
    {
        [HarmonyPatch(typeof(ClosedBoltWeapon), "BeginChamberingRound")]
        [HarmonyPatch(typeof(OpenBoltReceiver), "BeginChamberingRound")]
        [HarmonyPatch(typeof(Handgun), "ExtractRound")]
        [HarmonyPrefix]
        public static bool FTFPatch(FVRFireArm instance)
        {
            float failureinc = 0;
            if (!ConfigEntries.EnableAmmunitionFailures) return true;
            float rand = Common.GetRandomValue(0, 10001) / 100;
            if (instance.Magazine != null && ConfigEntries.EnableMagUnreliability)
                if (!instance.Magazine.IsBeltBox)
                    if (instance.Magazine.m_capacity > ConfigEntries.MinimumRoundCount)
                    {
                        float baseFailureInc = (instance.Magazine.m_capacity - ConfigEntries.MinimumRoundCount) *
                                               ConfigEntries.FailureIncrementPerRound;
                        failureinc = baseFailureInc + (baseFailureInc * ConfigEntries.GeneralMultiplier -
                                                       1 * ConfigEntries.MagUnreliabilityGeneralMultiplierEffect);
                    }

            float chance = ConfigEntries.HFRate * ConfigEntries.GeneralMultiplier + failureinc;
            return !(rand <= chance);
        }
    }
}