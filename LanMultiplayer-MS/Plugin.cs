using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using LanMultiplayerMS.Integrations;

// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace LanMultiplayerMS;

[ModInitializer("Initialize")]
public class Plugin
{
    private static void Initialize()
    {
        new Harmony("LanMultiplayerMS").PatchAll();
        ModConfigBridge.DeferredRegister();
    }
}