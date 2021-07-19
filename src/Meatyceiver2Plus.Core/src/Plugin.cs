using BepInEx;
using FistVR;
using HarmonyLib;
using Meatyceiver2Plus.Failures.Ammo;
using Meatyceiver2Plus.Failures.Firearm;
using UnityEngine;

#pragma warning disable 8618

namespace Meatyceiver2Plus
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
    [BepInProcess("h3vr.exe")]
    public class Plugin : BaseUnityPlugin
    {
        internal static Plugin Mod { get; private set; }

        public Plugin()
        {
            Logger.LogInfo("Meatyceiver2 started!");
            
            Harmony.CreateAndPatchAll(typeof(AmmoFailures));
            Harmony.CreateAndPatchAll(typeof(FirearmFailures));

            Mod = this;
        }
    }
}