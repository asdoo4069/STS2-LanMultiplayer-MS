using Godot;

namespace LanMultiplayerMS.Helpers;

internal static class LanMultiplayerUtil
{
    public static bool IsMobilePlatform()
    {
        return OS.GetName() == "Android" || OS.GetName() == "iOS";
    }
}