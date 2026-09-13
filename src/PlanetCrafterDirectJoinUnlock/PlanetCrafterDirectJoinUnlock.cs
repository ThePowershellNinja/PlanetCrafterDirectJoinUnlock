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
        public const string PluginVersion = "1.0.0";

        internal static ManualLogSource Log;

        private void Awake()
        {
            Log = Logger;
            var harmony = new Harmony(PluginGuid);
            harmony.PatchAll(typeof(Plugin).Assembly);
            Logger.LogInfo("Loaded Planet Crafter Direct Join Unlock.");
        }

        internal static void ApplyJoinMenuState(object instance)
        {
            if (instance == null)
            {
                return;
            }

            try
            {
                var instanceType = instance.GetType();
                var multiplayerButtonField = AccessTools.Field(instanceType, "multiplayerButton");
                var joinCodeMenuField = AccessTools.Field(instanceType, "multiplayerJoindCodeMenu");

                var multiplayerButton = multiplayerButtonField == null ? null : multiplayerButtonField.GetValue(instance) as GameObject;
                var joinCodeMenu = joinCodeMenuField == null ? null : joinCodeMenuField.GetValue(instance) as GameObject;

                if (multiplayerButton != null && !multiplayerButton.activeSelf)
                {
                    multiplayerButton.SetActive(true);
                    Log.LogInfo("Enabled direct-address multiplayer button.");
                }

                if (joinCodeMenu != null && joinCodeMenu.activeSelf)
                {
                    joinCodeMenu.SetActive(false);
                    Log.LogInfo("Disabled invite-code join menu.");
                }
            }
            catch (Exception ex)
            {
                Log.LogError("Failed to toggle join menus: " + ex);
            }
        }

        internal static void ForceOfflineDirectJoinMode()
        {
            try
            {
                var managersType = AccessTools.TypeByName("SpaceCraft.Managers");
                var savedDataHandlerType = AccessTools.TypeByName("SpaceCraft.SavedDataHandler");
                if (managersType == null || savedDataHandlerType == null)
                {
                    return;
                }

                var genericGetManager = AccessTools.Method(managersType, "GetManager");
                if (genericGetManager == null)
                {
                    return;
                }

                var getManager = genericGetManager.MakeGenericMethod(savedDataHandlerType);
                var savedData = getManager.Invoke(null, null);
                if (savedData == null)
                {
                    return;
                }

                var setOnlineGame = AccessTools.Method(savedDataHandlerType, "set_onlineGame");
                if (setOnlineGame != null)
                {
                    setOnlineGame.Invoke(savedData, new object[] { false });
                    Log.LogInfo("Forced direct join into offline/UnityTransport mode.");
                }
            }
            catch (Exception ex)
            {
                Log.LogError("Failed to force offline direct join mode: " + ex);
            }
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
                ApplyJoinMenuState(__instance);
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
                ApplyJoinMenuState(__instance);
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
                ForceOfflineDirectJoinMode();
            }
        }
    }
}
