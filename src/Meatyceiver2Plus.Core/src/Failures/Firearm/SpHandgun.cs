using FistVR;
using HarmonyLib;
using UnityEngine;

namespace Meatyceiver2Plus.Failures.Firearm
{
    public static partial class FirearmFailures
    {
        [HarmonyPatch(typeof(Handgun), "UpdateDisplayRoundPositions")]
        [HarmonyPostfix]
        public static void SpHandgun(Handgun instance, FVRFirearmMovingProxyRound mProxy)
        {
            if (instance.Slide.RotationInterpSpeed == 2)
            {
                mProxy.ProxyRound.transform.localPosition = Vector3.Lerp(
                    instance.Slide.Point_Slide_Forward.transform.position,
                    instance.Slide.Point_Slide_Rear.transform.position, ConfigEntries.StovepipeLerp);
            }
        }
    }
}