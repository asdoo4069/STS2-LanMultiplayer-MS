using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

namespace SlayTheSpire2.LAN.Multiplayer.Reforged.Models
{
    public struct LanPlayerNameRequestMessage : INetMessage, IPacketSerializable
    {
        public string playerName;

        public readonly bool ShouldBroadcast => false;
        public readonly bool ShouldBuffer => false;
        public readonly NetTransferMode Mode => NetTransferMode.Reliable;
        public readonly LogLevel LogLevel => LogLevel.Info;

        public readonly void Serialize(PacketWriter writer)
        {
            writer.WriteString(playerName);
        }

        public void Deserialize(PacketReader reader)
        {
            playerName = reader.ReadString();
        }
    }
}