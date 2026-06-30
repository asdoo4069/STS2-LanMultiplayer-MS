using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Saves;
using Steamworks;

namespace LanMultiplayerMS.Models
{
    [JsonSerializable(typeof(SettingsModel))]
    public partial class SettingsModelContext : JsonSerializerContext;

    public class SettingsModel : ISaveSchema
    {
        [JsonPropertyName("host_port")] public ushort HostPort { get; set; } = 33771;
        [JsonPropertyName("host_max_players")] public int HostMaxPlayers { get; set; } = 4;
        [JsonPropertyName("ip_address")] public string IPAddress { get; set; } = "127.0.0.1";
        [JsonPropertyName("remember_join_address")] public bool RememberJoinAddress { get; set; } = true;
        [JsonPropertyName("connect_timeout_seconds")] public int ConnectTimeoutSeconds { get; set; } = 10;
        [JsonPropertyName("net_id")] public ulong NetId { get; set; } = 1000u;
        [JsonPropertyName("player_name")] public string PlayerName { get; set; } = GetDefaultPlayerName();

        public int SchemaVersion { get; set; }

        private static string GetDefaultPlayerName()
        {
            try { return SteamFriends.GetPersonaName(); }
            catch { return "Player"; }
        }
    }
}