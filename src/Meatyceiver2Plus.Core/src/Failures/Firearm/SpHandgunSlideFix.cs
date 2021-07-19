using FistVR;
using HarmonyLib;
using UnityEngine;

namespace Meatyceiver2Plus.Failures.Firearm
{
    public static partial class FirearmFailures
    {
        [HarmonyPatch(typeof(HandgunSlide), "UpdateSlide")]
        [HarmonyPostfix]
        public static void SpHandgunSlideFix(HandgunSlide instance, float mSlideZCurrent, float state)
        {
            if (instance.GameObject.transform.localPosition.z >= state && instance.RotationInterpSpeed == 2)
            {
                var localPosition = instance.GameObject.transform.localPosition;
                localPosition = new Vector3(localPosition.x,
                    localPosition.y, state);
                instance.GameObject.transform.localPosition = localPosition;
                instance.Handgun.Chamber.UpdateProxyDisplay();
            }
        }
    }
}