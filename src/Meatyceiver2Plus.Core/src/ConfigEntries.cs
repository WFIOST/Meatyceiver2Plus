using BepInEx;
using BepInEx.Configuration;

namespace Meatyceiver2Plus
{
    public struct ConfigEntries
    {
        public static float BreakActionFTE => _breakActionFTE.Value;
        private static ConfigEntry<float> _breakActionFTE;
        
        public static float BreakActionFTEMultiplierEffect => _breakActionFTEMultiplierEffect.Value;
        private static ConfigEntry<float> _breakActionFTEMultiplierEffect;

        public static bool EnableAmmunitionFailures => _enableAmmunitionFailures.Value;
        private static ConfigEntry<bool> _enableAmmunitionFailures;

        public static bool EnableBrokenFirearmFailures => _enableAmmunitionFailures.Value;
        private static ConfigEntry<bool> _enableBrokenFirearmFailures;

        //General Settings

        public static bool EnableFirearmFailures => _enableAmmunitionFailures.Value;
        private static ConfigEntry<bool> _enableFirearmFailures;

        //Secondary Failure - Mag Unreliability

        public static bool EnableMagUnreliability => _enableMagUnreliability.Value;
        private static ConfigEntry<bool> _enableMagUnreliability;

        public static float FailureIncrementPerRound => _failureIncPerRound.Value;
        private static ConfigEntry<float> _failureIncPerRound;

        public static float FTERate => _fteRate.Value;
        private static ConfigEntry<float> _fteRate;

        //Failures - Firearms
        public static float FTLSlide => _ftlSlide.Value;
        private static ConfigEntry<float> _ftlSlide;

        //Multipliers
        public static float GeneralMultiplier => _generalMultiplier.Value;
        private static ConfigEntry<float> _generalMultiplier;

        //Failures - Broken Firearm
        public static float HFRate => _hfRate.Value;
        private static ConfigEntry<float> _hfRate;


        //Failures - Ammo
        public static float LPSFailureRate => _lpsFailureRate.Value;
        private static ConfigEntry<float> _lpsFailureRate;

        public static float MagUnreliabilityGeneralMultiplierEffect => _magUnreliabilityGeneralMultiplierEffect.Value;
        private static ConfigEntry<float> _magUnreliabilityGeneralMultiplierEffect;

        public static int MinimumRoundCount => _minRoundCount.Value;
        private static ConfigEntry<int> _minRoundCount;

        public static float RevolverFTE => _revolverFTE.Value;
        private static ConfigEntry<float> _revolverFTE;

        public static float RevolverFTEGeneralMultiplierEffect => _revolverFTEGeneralMultiplierEffect.Value;
        private static ConfigEntry<float> _revolverFTEGeneralMultiplierEffect;

        public static float SlamfireRate => _slamfireRate.Value;
        private static ConfigEntry<float> _slamfireRate;

        public static float StovepipeLerp => _stovepipeLerp.Value;
        private static ConfigEntry<float> _stovepipeLerp;

        static ConfigEntries()
        {
            _enableAmmunitionFailures = Plugin.Mod.Config.Bind(Strings.GeneralSettings, Strings.EnableAmmunitionFailures_key, true,
                Strings.EnableAmmunitionFailures_description);
            _enableFirearmFailures = Plugin.Mod.Config.Bind(Strings.GeneralSettings, Strings.EnableFirearmFailures_key, true,
                Strings.EnableFirearmFailures_description);
            _enableBrokenFirearmFailures = Plugin.Mod.Config.Bind(Strings.GeneralSettings, Strings.EnableBrokenFirearmFailures_key,
                true, Strings.EnableBrokenFirearmFailures_description);

            _generalMultiplier = Plugin.Mod.Config.Bind(Strings.GeneralMultipliers_section, Strings.GeneralMultipliers_key, 1f,
                Strings.GeneralMultipliers_description);


            _enableMagUnreliability = Plugin.Mod.Config.Bind(Strings.MagUnreliability_section, Strings.MagReliability_key, true,
                Strings.MagReliability_description);
            _failureIncPerRound = Plugin.Mod.Config.Bind(Strings.MagUnreliability_section, Strings.MagReliabilityMult_key, 0.04f,
                Strings.MagReliabilityMult_description);
            _minRoundCount = Plugin.Mod.Config.Bind(Strings.MagUnreliability_section, Strings.MinRoundCount_key, 15,
                Strings.MinRoundCount_description);
            _magUnreliabilityGeneralMultiplierEffect = Plugin.Mod.Config.Bind(Strings.MagUnreliability_section,
                Strings.MagUnreliabilityMult_key, 0.5f, Strings.MagUnreliabilityMult_description);
            _lpsFailureRate = Plugin.Mod.Config.Bind(Strings.AmmoFailures_section, Strings.LPSRate_key, 0.25f,
                Strings.ValidInput_float);
            Plugin.Mod.Config.Bind(Strings.AmmoFailures_section, Strings.HangFireRate_key, 0.1f,
                Strings.ValidInput_float);
            _fteRate = Plugin.Mod.Config.Bind(Strings.FirearmFailures_section, Strings.FTERate_key, 0.15f,
                Strings.ValidInput_float);
            Plugin.Mod.Config.Bind(Strings.FirearmFailures_section, Strings.StovepipeRate_key, 0.1f,
                Strings.ValidInput_float);
            _stovepipeLerp = Plugin.Mod.Config.Bind(Strings.FirearmFailures_section, Strings.StovepipeLerp_key, 0.5f,
                Strings.DEBUG);

            _hfRate = Plugin.Mod.Config.Bind(Strings.BrokenFirearmFailure, Strings.HFRate_key, 0.1f, Strings.ValidInput_float);
            _ftlSlide = Plugin.Mod.Config.Bind(Strings.BrokenFirearmFailure, Strings.FTLSlide_key, 5f, Strings.ValidInput_float);
            _slamfireRate = Plugin.Mod.Config.Bind(Strings.BrokenFirearmFailure, Strings.SlamFireRate_key, 0.1f,
                Strings.ValidInput_float);

            _breakActionFTE = Plugin.Mod.Config.Bind(Strings.BespokeFailure, Strings.BreakActionFTE_key, 30f,
                Strings.ValidInput_float);
            _breakActionFTEMultiplierEffect = Plugin.Mod.Config.Bind(Strings.BespokeFailure, Strings.BreakActionFTEMult_key, 0.5f,
                Strings.FTEMult_description);
            _revolverFTE = Plugin.Mod.Config.Bind(Strings.BespokeFailure, Strings.RevolverFTE_key, 30f, Strings.ValidInput_float);
            _revolverFTEGeneralMultiplierEffect = Plugin.Mod.Config.Bind(Strings.BespokeFailure, Strings.RevolverFTERate_key, 0.5f,
                Strings.FTEMult_description);
        }
    }
}