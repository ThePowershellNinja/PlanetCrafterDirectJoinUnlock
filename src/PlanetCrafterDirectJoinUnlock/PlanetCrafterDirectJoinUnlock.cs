using System;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace PlanetCrafterDirectJoinUnlock
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public sealed class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "net.justtoclayrify.planetcrafter.directjoinunlock";
        public const string PluginName = "Planet Crafter Direct Join Unlock";
        public const string PluginVersion = "1.1.0";

        internal static ManualLogSource Log;

        private void Awake()
        {
            Log = Logger;
            var harmony = new Harmony(PluginGuid);
            harmony.PatchAll(typeof(Plugin).Assembly);
            Logger.LogInfo("Loaded Planet Crafter Direct Join Unlock.");
        }

        internal static void ApplyDualJoinMenuState(object instance)
        {
            if (instance == null)
            {
                return;
            }

            try
            {
                var instanceType = instance.GetType();
                var multiplayerButtonField = AccessTools.Field(instanceType, "multiplayerButton");
                var multiplayerButton = multiplayerButtonField == null ? null : multiplayerButtonField.GetValue(instance) as GameObject;

                if (multiplayerButton != null && !multiplayerButton.activeSelf)
                {
                    multiplayerButton.SetActive(true);
                    Log.LogInfo("Enabled the direct-address multiplayer button alongside the standard invite-code flow.");
                }
            }
            catch (Exception ex)
            {
                Log.LogError("Failed to enable the direct-address multiplayer button: " + ex);
            }
        }

        internal static void PrepareDirectJoinMode()
        {
            try
            {
                var savedData = GetSavedDataHandler();
                if (savedData == null)
                {
                    return;
                }

                var setOnlineGame = AccessTools.Method(savedData.GetType(), "set_onlineGame");
                if (setOnlineGame == null)
                {
                    return;
                }

                setOnlineGame.Invoke(savedData, new object[] { false });
                Log.LogInfo("Prepared the direct-address join path without changing Steam or invite-code joining.");
            }
            catch (Exception ex)
            {
                Log.LogError("Failed to prepare the direct-address join path: " + ex);
            }
        }

        private static object GetSavedDataHandler()
        {
            var managersType = AccessTools.TypeByName("SpaceCraft.Managers");
            var savedDataHandlerType = AccessTools.TypeByName("SpaceCraft.SavedDataHandler");
            if (managersType == null || savedDataHandlerType == null)
            {
                return null;
            }

            var genericGetManager = AccessTools.Method(managersType, "GetManager");
            if (genericGetManager == null)
            {
                return null;
            }

            var getManager = genericGetManager.MakeGenericMethod(savedDataHandlerType);
            return getManager.Invoke(null, null);
        }

        [HarmonyPatch]
        private static class IntroStartPatch
        {
            private static MethodBase TargetMethod()
            {
                var introType = AccessTools.TypeByName("SpaceCraft.Intro");
                return introType == null ? null : AccessTools.Method(introType, "Start");
            }

            private static void Postfix(object __instance)
            {
                ApplyDualJoinMenuState(__instance);
            }
        }

        [HarmonyPatch]
        private static class IntroShowMainMenuPatch
        {
            private static MethodBase TargetMethod()
            {
                var introType = AccessTools.TypeByName("SpaceCraft.Intro");
                return introType == null ? null : AccessTools.Method(introType, "ShowMainMenu");
            }

            private static void Postfix(object __instance)
            {
                ApplyDualJoinMenuState(__instance);
            }
        }

        [HarmonyPatch]
        private static class MultiplayerMenuJoinPatch
        {
            private static MethodBase TargetMethod()
            {
                var multiplayerMenuType = AccessTools.TypeByName("SpaceCraft.MultiplayerMenu");
                return multiplayerMenuType == null ? null : AccessTools.Method(multiplayerMenuType, "Join");
            }

            private static void Prefix()
            {
                PrepareDirectJoinMode();
            }
        }
    }
}
