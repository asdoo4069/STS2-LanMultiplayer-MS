using System.Text.Json.Serialization;

namespace LanMultiplayerMS.Models
{
    [JsonSerializable(typeof(PlayerNames))]
    public partial class PlayerNamesContext : JsonSerializerContext;

    public class PlayerNames : Dictionary<ulong, string>;
}