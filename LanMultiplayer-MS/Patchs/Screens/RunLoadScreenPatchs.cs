using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Nodes.Screens.CustomRun;
using MegaCrit.Sts2.Core.Nodes.Screens.DailyRun;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Platform;
using LanMultiplayerMS.Helpers;
using LanMultiplayerMS.Services;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace LanMultiplayerMS.Patchs.Screens
{
    [HarmonyPatch]
    internal class RunLoadScreenInitializeAsHostPatchs
    {
        private static IEnumerable<MethodInfo> TargetMethods()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;

            yield return typeof(NMultiplayerLoadGameScreen).GetMethod("InitializeAsHost", flags)!;

            if (!LanMultiplayerUtil.IsMobilePlatform())
            {
                yield return typeof(NDailyRunLoadScreen).GetMethod("InitializeAsHost", flags)!;
                yield return typeof(NCustomRunLoadScreen).GetMethod("InitializeAsHost", flags)!;
            }
        }

        [HarmonyPrefix]
        private static void Prefix(NSubmenu __instance, INetGameService gameService)
        {
            if (gameService.Platform == PlatformType.None)
            {
                var runScreenService = RunScreenService.Instance;

                switch (__instance)
                {
                    case NMultiplayerLoadGameScreen multiplayerLoadGameScreen:
                        runScreenService.MultiplayerLoadGameScreen = multiplayerLoadGameScreen;
                        break;
                    case NDailyRunLoadScreen dailyRunLoadScreen:
                        runScreenService.DailyRunLoadScreen = dailyRunLoadScreen;
                        break;
                    case NCustomRunLoadScreen customRunLoadScreen:
                        runScreenService.CustomRunLoadScreen = customRunLoadScreen;
                        break;
                }
            }
        }
    }

    [HarmonyPatch]
    internal class RunLoadScreenInitializeAsClientPatchs
    {
        private static IEnumerable<MethodInfo> TargetMethods()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;

            yield return typeof(NMultiplayerLoadGameScreen).GetMethod("InitializeAsClient", flags)!;

            if (!LanMultiplayerUtil.IsMobilePlatform())
            {
                yield return typeof(NDailyRunLoadScreen).GetMethod("InitializeAsClient", flags)!;
                yield return typeof(NCustomRunLoadScreen).GetMethod("InitializeAsClient", flags)!;
            }
        }

        [HarmonyPrefix]
        private static void Prefix(NSubmenu __instance, INetGameService gameService)
        {
            if (gameService.Platform == PlatformType.None)
            {
                var runScreenService = RunScreenService.Instance;

                switch (__instance)
                {
                    case NMultiplayerLoadGameScreen multiplayerLoadGameScreen:
                        runScreenService.MultiplayerLoadGameScreen = multiplayerLoadGameScreen;
                        break;
                    case NDailyRunLoadScreen dailyRunLoadScreen:
                        runScreenService.DailyRunLoadScreen = dailyRunLoadScreen;
                        break;
                    case NCustomRunLoadScreen customRunLoadScreen:
                        runScreenService.CustomRunLoadScreen = customRunLoadScreen;
                        break;
                }
            }
        }
    }

    [HarmonyPatch]
    internal class RunLoadScreenShouldAllowRunToBeginPatchs
    {
        [HarmonyPrepare]
        private static bool Prepare()
        {
            return !LanMultiplayerUtil.IsMobilePlatform();
        }

        private static IEnumerable<MethodInfo> TargetMethods()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
            yield return typeof(NDailyRunLoadScreen).GetMethod("ShouldAllowRunToBegin", flags)!;
            yield return typeof(NCustomRunLoadScreen).GetMethod("ShouldAllowRunToBegin", flags)!;
        }

        [HarmonyPrefix]
        private static bool Prefix(LoadRunLobby ____lobby, ref Task<bool> __result)
        {
            if (____lobby.NetService.Platform == PlatformType.None)
            {
                __result = TaskGenericHelper.RunSafely(RunScreenService.ShouldAllowRunToBegin(____lobby));
                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(NMultiplayerLoadGameScreen), "ShouldAllowRunToBegin")]
    internal class RunLoadScreenShouldAllowRunToBeginPatch
    {
        [HarmonyPrefix]
        private static bool Prefix(LoadRunLobby ____runLobby, ref Task<bool> __result)
        {
            if (____runLobby.NetService.Platform == PlatformType.None)
            {
                __result = TaskGenericHelper.RunSafely(RunScreenService.ShouldAllowRunToBegin(____runLobby));
                return false;
            }

            return true;
        }
    }
}