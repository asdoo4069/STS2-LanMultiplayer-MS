using Godot;

namespace SlayTheSpire2.LAN.Multiplayer.Reforged.Helpers;

internal static class LanMultiplayerUtil
{
    public static bool IsMobilePlatform()
    {
        return OS.GetName() == "Android" || OS.GetName() == "iOS";
    }
}