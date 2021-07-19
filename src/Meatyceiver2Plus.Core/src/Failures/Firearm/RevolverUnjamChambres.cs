using FistVR;
using HarmonyLib;

namespace Meatyceiver2Plus.Failures.Firearm
{
    public static partial class FirearmFailures
    {
        [HarmonyPatch(typeof(Revolver), "UpdateCylinderRelease")]
        [HarmonyPostfix]
        public static void RevolverUnjamChambers(Revolver instance)
        {
            float z = instance.transform.InverseTransformDirection(instance.m_hand.Input.VelLinearWorld).z;
            if (z > 0f)
                foreach (var chamber in instance.Chambers)
                    chamber.RotationInterpSpeed = 1;
        }
    }
}