﻿using System;
using Infusion.Packets;
using Infusion.Packets.Client;

namespace Infusion
{
    internal interface IClientPacketSubject
    {
        void RegisterFilter(Func<Packet, Packet?> filter);
        void RegisterOutputFilter(Func<Packet, Packet?> filter);

        void Subscribe<TPacket>(PacketDefinition<TPacket> definition, Action<TPacket> observer)
            where TPacket : MaterializedPacket;

        void Unsubscribe<TPacket>(PacketDefinition<TPacket> definition, Action<TPacket> observer)
            where TPacket : MaterializedPacket;
    }
}