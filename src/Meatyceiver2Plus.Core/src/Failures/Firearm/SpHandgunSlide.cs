using FistVR;
using HarmonyLib;

namespace Meatyceiver2Plus.Failures.Firearm
{
    public static partial class FirearmFailures
    {
        [HarmonyPatch(typeof(HandgunSlide), "UpdateSlide")]
        [HarmonyPrefix]
        public static bool SpHandgunSlide(
            HandgunSlide instance,
            float mSlideZForward,
            float mSlideZRear,
            float mSlideZCurrent,
            float mCurSlideSpeed,
            out float state
        )
        {
            if (instance.RotationInterpSpeed == 2)
            {
                mSlideZCurrent = mSlideZForward - (mSlideZForward - mSlideZRear) / 2;
                if (instance.CurPos == HandgunSlide.SlidePos.LockedToRear)
                {
                    instance.RotationInterpSpeed = 1;
                }
            }

            state = mSlideZCurrent;
            return true;
        }
    }
}