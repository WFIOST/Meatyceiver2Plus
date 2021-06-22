using BepInEx;
using BepInEx.Configuration;
using FistVR;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

#pragma warning disable 8618

namespace Meatyceiver2Plus
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
    [BepInProcess("h3vr.exe")]
    public class Plugin : BaseUnityPlugin
    {
        [FormerlySerializedAs("RandomVar")] public Random randomVar;

        
        public readonly ConfigEntry<bool> EnableConsoleDebugging;
        
        
        internal static Plugin Instance { get; private set; }

        //Bespoke Failures

        private readonly ConfigEntry<float> _breakActionFte;
        private readonly ConfigEntry<float> _breakActionFteMultAffect;
        private ConfigEntry<float> _dfRate;
        private readonly ConfigEntry<bool> _enableAmmunitionFailures;
        private readonly ConfigEntry<bool> _enableBrokenFirearmFailures;

        //General Settings

        private readonly ConfigEntry<bool> _enableFirearmFailures;

        //Secondary Failure - Mag Unreliability

        private readonly ConfigEntry<bool> _enableMagUnreliability;
        private readonly ConfigEntry<float> _failureIncPerRound;
        private readonly ConfigEntry<float> _fteRate;

        //Failures - Firearms

        private ConfigEntry<float> _ftfRate;
        private readonly ConfigEntry<float> _ftlSlide;

        //Multipliers

        private readonly ConfigEntry<float> _generalMult;

        //Failures - Broken Firearm

        private readonly ConfigEntry<float> _hfRate;


        //Failures - Ammo

        private readonly ConfigEntry<float> _lpsFailureRate;
        private readonly ConfigEntry<float> _magUnreliabilityGenMultAffect;
        private readonly ConfigEntry<int> _minRoundCount;

        private readonly ConfigEntry<float> _revolverFte;
        private readonly ConfigEntry<float> _revolverFteGenMultAffect;
        private readonly ConfigEntry<float> _slamfireRate;
        private readonly ConfigEntry<float> _stovepipeLerp;

        public Plugin()
        {
            Logger.LogInfo("Meatyceiver2 started!");

            _enableAmmunitionFailures = Config.Bind(Strings.GeneralSettings, Strings.EnableAmmunitionFailures_key, true,
                Strings.EnableAmmunitionFailures_description);
            _enableFirearmFailures = Config.Bind(Strings.GeneralSettings, Strings.EnableFirearmFailures_key, true,
                Strings.EnableFirearmFailures_description);
            _enableBrokenFirearmFailures = Config.Bind(Strings.GeneralSettings, Strings.EnableBrokenFirearmFailures_key,
                true, Strings.EnableBrokenFirearmFailures_description);
            EnableConsoleDebugging = Config.Bind(Strings.GeneralSettings, Strings.EnableConsoleDebugging_key, false,
                Strings.EnableConsoleDebugging_description);

            _generalMult = Config.Bind(Strings.GeneralMultipliers_section, Strings.GeneralMultipliers_key, 1f,
                Strings.GeneralMultipliers_description);


            _enableMagUnreliability = Config.Bind(Strings.MagUnreliability_section, Strings.MagReliability_key, true,
                Strings.MagReliability_description);
            _failureIncPerRound = Config.Bind(Strings.MagUnreliability_section, Strings.MagReliabilityMult_key, 0.04f,
                Strings.MagReliabilityMult_description);
            _minRoundCount = Config.Bind(Strings.MagUnreliability_section, Strings.MinRoundCount_key, 15,
                Strings.MinRoundCount_description);
            _magUnreliabilityGenMultAffect = Config.Bind(Strings.MagUnreliability_section,
                Strings.MagUnreliabilityMult_key, 0.5f, Strings.MagUnreliabilityMult_description);

            //enableLongTermBreakdown = Config.Bind(Strings.LongTermBreak_section, Strings.LongTermBreak_key, true, Strings.LongTermBreak_description);

            _lpsFailureRate = Config.Bind(Strings.AmmoFailures_section, Strings.LPSRate_key, 0.25f,
                Strings.ValidInput_float);
            Config.Bind(Strings.AmmoFailures_section, Strings.HangFireRate_key, 0.1f,
                Strings.ValidInput_float);

            _ftfRate = Config.Bind(Strings.FirearmFailures_section, Strings.FTFRate_key, 0.25f,
                Strings.ValidInput_float);
            _fteRate = Config.Bind(Strings.FirearmFailures_section, Strings.FTERate_key, 0.15f,
                Strings.ValidInput_float);
            _dfRate = Config.Bind(Strings.FirearmFailures_section, Strings.DFRate_key, 0.15f, Strings.ValidInput_float);
            Config.Bind(Strings.FirearmFailures_section, Strings.StovepipeRate_key, 0.1f,
                Strings.ValidInput_float);
            _stovepipeLerp = Config.Bind(Strings.FirearmFailures_section, Strings.StovepipeLerp_key, 0.5f,
                Strings.DEBUG);

            _hfRate = Config.Bind(Strings.BrokenFirearmFailure, Strings.HFRate_key, 0.1f, Strings.ValidInput_float);
            _ftlSlide = Config.Bind(Strings.BrokenFirearmFailure, Strings.FTLSlide_key, 5f, Strings.ValidInput_float);
            _slamfireRate = Config.Bind(Strings.BrokenFirearmFailure, Strings.SlamFireRate_key, 0.1f,
                Strings.ValidInput_float);

            _breakActionFte = Config.Bind(Strings.BespokeFailure, Strings.BreakActionFTE_key, 30f,
                Strings.ValidInput_float);
            _breakActionFteMultAffect = Config.Bind(Strings.BespokeFailure, Strings.BreakActionFTEMult_key, 0.5f,
                Strings.FTEMult_description);
            _revolverFte = Config.Bind(Strings.BespokeFailure, Strings.RevolverFTE_key, 30f, Strings.ValidInput_float);
            _revolverFteGenMultAffect = Config.Bind(Strings.BespokeFailure, Strings.RevolverFTERate_key, 0.5f,
                Strings.FTEMult_description);


            Harmony.CreateAndPatchAll(typeof(Plugin));
            randomVar = new Random();

            Instance = this;
        }



        //BEGIN AMMO FAILURES
        [HarmonyPatch(typeof(FVRFireArmChamber), "Fire")]
        [HarmonyPrefix]
        private bool LightPrimerStrike(ref bool result, FVRFireArmChamber instance, FVRFireArmRound mRound)
        {
            const string failureName = "LPS";
            if (!_enableAmmunitionFailures.Value) return true;
            if (instance.Firearm is Revolver || instance.Firearm is RevolvingShotgun) return true;
            var rand = (float) randomVar.Next(0, 10001) / 100;
            var chance = _lpsFailureRate.Value * _generalMult.Value;
            Debug.ConsoleDebugging(0, failureName, rand, chance);
            //			if (enableConsoleDebugging.Value) { Debug.Log("LPS RNG: " + rand + " to " + LPSFailureRate.Value * generalMult.Value); }
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
            else
            {
                Debug.ConsoleDebugging(1, failureName, rand, chance);
            }

            result = false;
            return false;
        }

        [HarmonyPatch(typeof(Revolver), "Fire")]
        [HarmonyPrefix]
        private bool LpsRevolver(Revolver instance)
        {
            const string failureName = "LPS";
            if (!_enableAmmunitionFailures.Value) return true;
            var rand = (float) randomVar.Next(0, 10001) / 100;
            var chance = _lpsFailureRate.Value * _generalMult.Value;
            Debug.ConsoleDebugging(0, failureName, rand, chance);
            if (rand <= chance)
            {
                Debug.ConsoleDebugging(1, failureName, rand, chance);
                instance.Chambers[instance.CurChamber].IsSpent = false;
                instance.Chambers[instance.CurChamber].UpdateProxyDisplay();
                return false;
            }

            return true;
        }

        [HarmonyPatch(typeof(RevolvingShotgun), "Fire")]
        [HarmonyPrefix]
        private bool LpsRevolvingShotgun(RevolvingShotgun instance)
        {
            const string failureName = "LPS";
            if (!_enableAmmunitionFailures.Value) return true;
            var rand = (float) randomVar.Next(0, 10001) / 100;
            var chance = _lpsFailureRate.Value * _generalMult.Value;
            Debug.ConsoleDebugging(0, failureName, rand, chance);
            if (rand <= chance)
            {
                Debug.ConsoleDebugging(1, failureName, rand, chance);
                instance.Chambers[instance.CurChamber].IsSpent = false;
                instance.Chambers[instance.CurChamber].UpdateProxyDisplay();
                return false;
            }

            return true;
        }


        //BEGIN FIREARM FAILURES

        [HarmonyPatch(typeof(ClosedBoltWeapon), "BeginChamberingRound")]
        [HarmonyPatch(typeof(OpenBoltReceiver), "BeginChamberingRound")]
        [HarmonyPatch(typeof(Handgun), "ExtractRound")]
        [HarmonyPrefix]
        private bool FtfPatch(FVRFireArm instance)
        {
            const string failureName = "FTF";
            float failureinc = 0;
            if (!_enableFirearmFailures.Value) return true;
            var rand = (float) randomVar.Next(0, 10001) / 100;
            if (instance.Magazine != null && _enableMagUnreliability.Value)
                if (!instance.Magazine.IsBeltBox)
                    if (instance.Magazine.m_capacity > _minRoundCount.Value)
                    {
                        var baseFailureInc = (instance.Magazine.m_capacity - _minRoundCount.Value) *
                                             _failureIncPerRound.Value;
                        failureinc = baseFailureInc + (baseFailureInc * _generalMult.Value -
                                                       1 * _magUnreliabilityGenMultAffect.Value);
                    }

            var chance = _hfRate.Value * _generalMult.Value + failureinc;
            Debug.ConsoleDebugging(0, failureName, rand, chance);
            if (rand <= chance)
            {
                Debug.ConsoleDebugging(1, failureName, rand, chance);
                return false;
            }

            return true;
        }

        [HarmonyPatch(typeof(BreakActionWeapon), "PopOutRound")]
        [HarmonyPrefix]
        private bool FteEmptyBreakAction(BreakActionWeapon instance, FVRFireArm chamber)
        {
            const string failureName = "BA FTE";
            if (!_enableFirearmFailures.Value) return true;
            if (chamber.RotationInterpSpeed == 2) return false;
            var rand = (float) randomVar.Next(0, 10001) / 100;
            var chance = _breakActionFte.Value +
                         _breakActionFte.Value * (_generalMult.Value - 1) * _breakActionFteMultAffect.Value;
            Debug.ConsoleDebugging(0, failureName, rand, chance);
            if (rand <= chance)
            {
                Debug.ConsoleDebugging(1, failureName, rand, chance);
                chamber.RotationInterpSpeed = 2;
                return false;
            }

            return true;
        }

        [HarmonyPatch(typeof(Revolver), "UpdateCylinderRelease")]
        [HarmonyPostfix]
        private void RevolverUnjamChambers(Revolver instance)
        {
            var z = instance.transform.InverseTransformDirection(instance.m_hand.Input.VelLinearWorld).z;
            if (z > 0f)
                foreach (var chamber in instance.Chambers)
                    chamber.RotationInterpSpeed = 1;
        }

        [HarmonyPatch(typeof(FVRFireArmChamber), "EjectRound")]
        [HarmonyPrefix]
        private bool RevolverAndRollingBlockFte(FVRFireArmChamber instance)
        {
            if (!_enableFirearmFailures.Value) return true;
            switch (instance.Firearm)
            {
                case Revolver:
                {
                    if (instance.RotationInterpSpeed == 1)
                    {
                        string failureName = "Revolver FTE";
                        var rand = (float) randomVar.Next(0, 10001) / 100;
                        var chance = _revolverFte.Value +
                                     _revolverFte.Value * (_generalMult.Value - 1) * _revolverFteGenMultAffect.Value;
                        Debug.ConsoleDebugging(0, failureName, rand, chance);
                        if (rand <= chance)
                        {
                            Debug.ConsoleDebugging(1, failureName, rand, chance);
                            instance.RotationInterpSpeed = 2;
                            return false;
                        }
                    }

                    break;
                }
                case RollingBlock:
                {
                    const string failureName = "Rolling block FTE";
                    var rand = (float) randomVar.Next(0, 10001) / 100;
                    var chance = _breakActionFte.Value +
                                 _breakActionFte.Value * (_generalMult.Value - 1) * _breakActionFteMultAffect.Value;
                    Debug.ConsoleDebugging(0, failureName, rand, chance);
                    if (rand <= chance)
                    {
                        Debug.ConsoleDebugging(1, failureName, rand, chance);
                        return false;
                    }

                    break;
                }
            }

            return true;
        }

        [HarmonyPatch(typeof(FVRFireArmChamber), "Awake")]
        [HarmonyPrefix]
        private bool RollingBlockChamberAddEjectPointPatch(FVRFireArmChamber instance)
        {
            if (instance.Firearm is RollingBlock) instance.IsManuallyExtractable = true;
            return true;
        }

        [HarmonyPatch(typeof(FVRFireArmChamber), "BeginInteraction")]
        [HarmonyPostfix]
        private void BreakActionFteFix(FVRFireArmChamber instance)
        {
            instance.RotationInterpSpeed = 1;
        }

        [HarmonyPatch(typeof(ClosedBolt), "ImpartFiringImpulse")]
        [HarmonyPatch(typeof(HandgunSlide), "ImpartFiringImpulse")]
        [HarmonyPatch(typeof(OpenBoltReceiverBolt), "ImpartFiringImpulse")]
        [HarmonyPrefix]
        private bool FtePatch(FVRInteractiveObject instance)
        {
            const string stovePipeFailureName = "Stovepipe";
            if (instance is BoltActionRifle || instance is LeverActionFirearm) return false;
            if (!_enableFirearmFailures.Value) return true;
            var rand = (float) randomVar.Next(0, 10001) / 100;
            var chance = _fteRate.Value * _generalMult.Value;
            Debug.ConsoleDebugging(0, stovePipeFailureName, rand, chance);
            if (rand <= chance)
            {
                Debug.ConsoleDebugging(1, stovePipeFailureName, rand, chance);
                instance.RotationInterpSpeed = 2;
                return false;
            }

//			rand = (float)randomVar.Next(0, 10001) / 100;
//			chance = stovepipeRate.Value * generalMult.Value;
//			Debug.ConsoleDebugging(0, FTEfailureName, rand, chance);
//			if (rand <= chance)
//			{
//				Debug.ConsoleDebugging(1, FTEfailureName, rand, chance);
//				return false;
//			}
            return true;
        }

        [HarmonyPatch(typeof(HandgunSlide), "UpdateSlide")]
        [HarmonyPrefix]
        private bool SpHandgunSlide(
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
                UnityEngine.Debug.Log("prefix slidez: " + mSlideZCurrent);
                mCurSlideSpeed = 0;
                if (instance.CurPos == HandgunSlide.SlidePos.LockedToRear)
                {
                    instance.RotationInterpSpeed = 1;
                    UnityEngine.Debug.Log("Stovepipe cleared!");
                }
            }

            state = mSlideZCurrent;
            return true;
        }

        [HarmonyPatch(typeof(HandgunSlide), "UpdateSlide")]
        [HarmonyPostfix]
        private void SpHandgunSlideFix(HandgunSlide instance, float mSlideZCurrent, float state)
        {
            //			if (__instance.RotationInterpSpeed == 2) Debug.Log("prefix slidez: " + __state + " postfix slidez: " + ___m_slideZ_current);
            if (instance.GameObject.transform.localPosition.z >= state && instance.RotationInterpSpeed == 2)
            {
                var localPosition = instance.GameObject.transform.localPosition;
                localPosition = new Vector3(localPosition.x,
                    localPosition.y, state);
                instance.GameObject.transform.localPosition = localPosition;
                instance.Handgun.Chamber.UpdateProxyDisplay();
            }
        }

        [HarmonyPatch(typeof(Handgun), "UpdateDisplayRoundPositions")]
        [HarmonyPostfix]
        private void SpHandgun(Handgun instance, FVRFirearmMovingProxyRound mProxy)
        {
            if (instance.Slide.RotationInterpSpeed == 2)
            {
                UnityEngine.Debug.Log("lerping");
                mProxy.ProxyRound.transform.localPosition = Vector3.Lerp(
                    instance.Slide.Point_Slide_Forward.transform.position,
                    instance.Slide.Point_Slide_Rear.transform.position, _stovepipeLerp.Value);
            }
        }

        //BEGIN BROKEN FIREARM FAILURES

        [HarmonyPatch(typeof(HandgunSlide), "SlideEvent_ArriveAtFore")]
        [HarmonyPostfix]
        private void SfHandgun(HandgunSlide instance)
        {
            if (_enableBrokenFirearmFailures.Value)
            {
                string failureName = "Slam fire";
                var rand = (float) randomVar.Next(0, 10001) / 100;
                var chance = _slamfireRate.Value * _generalMult.Value;
                Debug.ConsoleDebugging(0, failureName, rand, chance);
                if (rand <= chance)
                {
                    Debug.ConsoleDebugging(1, failureName, rand, chance);
                    instance.Handgun.DropHammer(false);
                }
            }
        }

        [HarmonyPatch(typeof(ClosedBolt), "BoltEvent_ArriveAtFore")]
        [HarmonyPostfix]
        private void SfClosedBolt(ClosedBolt instance)
        {
            if (_enableBrokenFirearmFailures.Value)
            {
                const string failureName = "Slam fire";
                var rand = (float) randomVar.Next(0, 10001) / 100;
                var chance = _slamfireRate.Value * _generalMult.Value;
                Debug.ConsoleDebugging(0, failureName, rand, chance);
                if (rand <= chance)
                {
                    Debug.ConsoleDebugging(1, failureName, rand, chance);
                    instance.Weapon.DropHammer();
                }
            }
        }


        [HarmonyPatch(typeof(ClosedBoltWeapon), "CockHammer")]
        [HarmonyPrefix]
        private bool HFClosedBolt()
        {
            if (!_enableBrokenFirearmFailures.Value) return true;
            const string failureName = "Hammer follow";
            var rand = (float) randomVar.Next(0, 10001) / 100;
            var chance = _hfRate.Value * _generalMult.Value;
            Debug.ConsoleDebugging(0, failureName, rand, chance);
            if (rand <= chance)
            {
                Debug.ConsoleDebugging(1, failureName, rand, chance);
                return false;
            }

            return true;
        }

        [HarmonyPatch(typeof(Handgun), "CockHammer")]
        [HarmonyPrefix]
        private bool HFHandgun(bool isManual)
        {
            if (!_enableBrokenFirearmFailures.Value) return true;
            const string failureName = "Hammer follow";
            var rand = (float) randomVar.Next(0, 10001) / 100;
            var chance = _hfRate.Value * _generalMult.Value;
            Debug.ConsoleDebugging(0, failureName, rand, chance);
            if (rand <= chance && !isManual)
            {
                Debug.ConsoleDebugging(1, failureName, rand, chance);
                return false;
            }

            return true;
        }

        [HarmonyPatch(typeof(Handgun), "EngageSlideRelease")]
        [HarmonyPrefix]
        private bool FtlsHandgun()
        {
            if (!_enableBrokenFirearmFailures.Value) return true;
            const string failureName = "Failure to lock slide";
            var rand = (float) randomVar.Next(0, 10001) / 100;
            var chance = _ftlSlide.Value * _generalMult.Value;
            Debug.ConsoleDebugging(0, failureName, rand, chance);
            if (rand <= chance)
            {
                Debug.ConsoleDebugging(1, failureName, rand, chance);
                return false;
            }

            return true;
        }
    }
}