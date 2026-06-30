using MegaCrit.Sts2.Core.Multiplayer.Transport.ENet;

// ReSharper disable InconsistentNaming

namespace LanMultiplayerMS.Models
{
    public struct ENetLanHandshakeResponse
    {
        public ENetHandshakeStatus status;

        public ulong netId;

        public ulong newNetId;
    }
}