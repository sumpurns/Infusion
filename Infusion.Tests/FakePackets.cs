﻿using System;
using System.IO;
using Infusion.IO;
using Infusion.Packets;

namespace Infusion.Tests
{
    public static class FakePackets
    {
        public static Packet InitialLoginSeedPacket = new Packet(PacketDefinitions.LoginSeed.Id, InitialLoginSeed);

        public static Packet ConnectToGameServerPacket = Instantiate(ConnectToGameServer);

        public static Packet GameServerLoginRequestPacket = Instantiate(GameServerLoginRequest);

        public static byte[] CharactersStartingLocationsEncrypted =
        {
            0xE6, 0x6E, 0x31, 0xE2, 0xDC, 0x60, 0x8C, 0xD6, 0xFE, 0x7C, 0x25, 0x69, 0x05, 0x92, 0x66, 0x23, 0x67,
            0x14, 0xE3, 0x62, 0xDC, 0x60, 0x8C, 0xD6, 0xFE, 0x7C, 0x25, 0x69, 0x05, 0x92, 0x66, 0x23, 0x67, 0x14,
            0xE3, 0x62, 0xDC, 0x60, 0x8C, 0xD6, 0xFE, 0x7C, 0x25, 0x69, 0x05, 0x92, 0x66, 0x23, 0x67, 0x14, 0xE3,
            0x62, 0xDC, 0x60, 0x8C, 0xD6, 0xFE, 0x7C, 0x25, 0x69, 0x05, 0x92, 0x66, 0x23, 0x67, 0x14, 0xE3, 0x62,
            0xDC, 0x60, 0x8C, 0xD6, 0xFE, 0x7C, 0x25, 0x69, 0x05, 0x92, 0x6A, 0xCB, 0xEA, 0xE7, 0x9B, 0x62, 0xDC,
            0x60, 0x8C, 0xD6, 0xFE, 0x7E, 0xD7, 0x6E, 0x9E, 0xC3, 0x9E, 0x15, 0x5A, 0x27, 0xC3, 0x39, 0xF1, 0xFF,
            0x43, 0x2F, 0x99, 0x7C, 0x25, 0x69, 0x04, 0x66, 0x02, 0x3D, 0x59, 0x37, 0xA3, 0x62, 0xDC, 0x60, 0x8C,
            0xD6, 0xFF, 0x05, 0x26, 0xA4, 0xB6, 0x0F, 0x47, 0xD2, 0x8E, 0x0E, 0x36, 0x86, 0xDC, 0x60, 0x8C, 0xD6,
            0xF6, 0xDB, 0x04, 0xA8, 0xC8, 0x40, 0x69, 0x33, 0x67, 0x14, 0xE3, 0x62, 0xDC, 0x6E, 0xE1, 0x28, 0x82,
            0x9A, 0x92, 0x50, 0x0A, 0x5D, 0x2A, 0x34, 0x0C, 0x4C, 0x3F, 0x1A, 0x5C, 0x60, 0x8C, 0xD6, 0x2F, 0x65,
            0xC6, 0xAE, 0x12, 0x99, 0x31, 0xA8, 0xA7, 0x14, 0xE3, 0x62, 0xDC, 0x60, 0xD2, 0x96, 0x0D, 0x13, 0x13,
            0x5C, 0x06, 0x51, 0x31, 0x6B, 0x09, 0xC2, 0x52, 0xDA, 0x2D, 0x60, 0x8C, 0xD6, 0xFD, 0xD6, 0xD1, 0x51,
            0x39, 0xCF, 0xC6, 0xE5, 0xE7, 0x14, 0xE3, 0x62, 0xDC, 0x60, 0xA3, 0xF6, 0x87, 0xC9, 0x5F, 0x76, 0x41,
            0xC5, 0xF0, 0x8C, 0x57, 0xC8, 0xE4, 0x09, 0x84, 0xBC, 0xF4, 0x56, 0xFE, 0x7C, 0x31, 0x4A, 0x3F, 0xAA,
            0x06, 0xD2, 0xEA, 0x97, 0x33, 0x62, 0xDC, 0x60, 0x8C, 0xD6, 0xFB, 0x98, 0x2A, 0x5F, 0xA1, 0xF3, 0x9F,
            0xCA, 0xFD, 0xC0, 0xDC, 0xEA, 0xA0, 0x3D, 0x21, 0x88, 0x16, 0xF6, 0xD6, 0x66, 0x8D, 0x92, 0x67, 0xFE,
            0xBA, 0x17, 0x28, 0x35, 0x50, 0x70, 0x8C, 0xD6, 0xFE, 0x7C, 0x25, 0x68, 0x7C, 0x91, 0xAB, 0x8B, 0xA8,
            0x24, 0x00, 0x1E, 0x2D, 0x89, 0x81, 0x4B, 0x95, 0x67, 0xAA, 0x79, 0x05, 0x92, 0x66, 0x46, 0xAA, 0x4C,
            0x97, 0xE5, 0x0E, 0xB9, 0x44, 0xAB, 0xC2, 0xFC, 0x25, 0x69, 0x05, 0x92, 0x6D, 0xEB, 0x79, 0x79, 0x24,
            0x96, 0xB6, 0xED, 0x7D, 0x11, 0x32, 0x4B, 0x24, 0xB3, 0xD3, 0xA5, 0x78, 0x03, 0x67, 0x14, 0xE4, 0x66,
            0x73, 0xAB, 0x39, 0x59, 0x66, 0x1C, 0x25, 0x69, 0x05, 0x92, 0x66, 0x21, 0x95, 0x13, 0x78, 0x3A, 0x0C,
            0x9C, 0xFD, 0x69, 0x39, 0xF2, 0x6E, 0x31, 0xD9, 0xEA, 0xE6, 0x23, 0x67, 0x14, 0x27, 0xE5, 0x34, 0xEA,
            0x7F, 0x12, 0xFE, 0x7C, 0x25, 0x69, 0x05, 0x92, 0x41, 0x05, 0xA1, 0xB8, 0xF9, 0xDE, 0x1F, 0x10, 0x91,
            0x7B, 0xBD, 0xBD, 0xC1, 0x9A, 0xD7, 0x6C, 0xEA, 0xC3, 0x67, 0x14, 0xE1, 0xD0, 0xFF, 0x67, 0xB3, 0xD9,
            0x5E, 0x7C, 0x25, 0x69, 0x05, 0x92, 0x66, 0x00, 0x5D, 0x57, 0x07, 0x93, 0x45, 0x61, 0x4C, 0xA0, 0x4B,
            0x07, 0xE0, 0x97, 0x64, 0x12, 0x66, 0x23, 0x67, 0x28, 0x42, 0x98, 0xFE, 0xDC, 0x7D, 0xD6, 0xFE, 0x7C,
            0x25, 0x69, 0x05, 0x96, 0x23, 0x04, 0xAB, 0x2A, 0x7E, 0x5A, 0x64, 0x6E, 0x5A, 0x75, 0xC4, 0x0D, 0x53,
            0xEA, 0xE6, 0x1A, 0x66, 0x23, 0x67, 0x15, 0xAB
        };

        public static byte[] CharactersStartingLocations =
        {
            0xA9, 0x04, 0x29, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x0C, 0x00, 0x59, 0x65, 0x77, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x54, 0x68, 0x65, 0x20, 0x45, 0x6D, 0x70, 0x61, 0x74, 0x68, 0x20, 0x41, 0x62, 0x62, 0x65,
            0x79, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x01, 0x4D, 0x69, 0x6E, 0x6F, 0x63, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x54, 0x68, 0x65, 0x20, 0x42, 0x61, 0x72, 0x6E, 0x61, 0x63, 0x6C, 0x65, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02,
            0x42, 0x72, 0x69, 0x74, 0x61, 0x69, 0x6E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x53,
            0x77, 0x65, 0x65, 0x74, 0x20, 0x44, 0x72, 0x65, 0x61, 0x6D, 0x73, 0x20, 0x49, 0x6E, 0x6E, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x4D,
            0x6F, 0x6F, 0x6E, 0x67, 0x6C, 0x6F, 0x77, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x54, 0x68,
            0x65, 0x20, 0x53, 0x63, 0x68, 0x6F, 0x6C, 0x61, 0x72, 0x73, 0x20, 0x49, 0x6E, 0x6E, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x54, 0x72,
            0x69, 0x6E, 0x73, 0x69, 0x63, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x54, 0x68, 0x65,
            0x20, 0x54, 0x72, 0x61, 0x76, 0x65, 0x6C, 0x65, 0x72, 0x27, 0x73, 0x20, 0x49, 0x6E, 0x6E, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x4D, 0x61, 0x67,
            0x69, 0x6E, 0x63, 0x69, 0x61, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x54, 0x68, 0x65, 0x20,
            0x47, 0x72, 0x65, 0x61, 0x74, 0x20, 0x48, 0x6F, 0x72, 0x6E, 0x73, 0x20, 0x54, 0x61, 0x76, 0x65,
            0x72, 0x6E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x4A, 0x68, 0x65, 0x6C,
            0x6F, 0x6D, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x54, 0x68, 0x65, 0x20, 0x4D,
            0x65, 0x72, 0x63, 0x65, 0x6E, 0x61, 0x72, 0x79, 0x20, 0x49, 0x6E, 0x6E, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0x53, 0x6B, 0x61, 0x72, 0x61,
            0x20, 0x42, 0x72, 0x61, 0x65, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x54, 0x68, 0x65, 0x20, 0x46, 0x61,
            0x6C, 0x63, 0x6F, 0x6E, 0x65, 0x72, 0x27, 0x73, 0x20, 0x49, 0x6E, 0x6E, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x56, 0x65, 0x73, 0x70, 0x65, 0x72,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x54, 0x68, 0x65, 0x20, 0x49, 0x72, 0x6F,
            0x6E, 0x77, 0x6F, 0x6F, 0x64, 0x20, 0x49, 0x6E, 0x6E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x09, 0x48, 0x61, 0x76, 0x65, 0x6E, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x42, 0x75, 0x63, 0x6B, 0x6C, 0x65, 0x72, 0x27,
            0x73, 0x20, 0x48, 0x69, 0x64, 0x65, 0x61, 0x77, 0x61, 0x79, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0A, 0x55, 0x6D, 0x62, 0x72, 0x61, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4D, 0x61, 0x72, 0x64, 0x6F, 0x74, 0x68, 0x27, 0x73,
            0x20, 0x54, 0x6F, 0x77, 0x65, 0x72, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0B, 0x48, 0x61, 0x76, 0x65, 0x6E, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x55, 0x7A, 0x65, 0x72, 0x61, 0x61, 0x6E, 0x27, 0x73, 0x20,
            0x4D, 0x61, 0x6E, 0x73, 0x69, 0x6F, 0x6E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x28
        };

        public static byte[] InitialLoginSeed => new byte[] {0xA9, 0xFE, 0x50, 0x50};

        public static byte[] InitialLoginRequest => new byte[]
        {
            0x80, 0x61, 0x64, 0x6D, 0x69, 0x6E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x61, 0x64, 0x6D, 0x69, 0x6E,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x99
        };

        public static byte[] InitialLoginRequestEncrypted => new byte[]
        {
            0x7A, 0x63, 0x9A, 0xED, 0x56, 0x0E, 0x4F, 0xD8, 0x93, 0x36, 0x64, 0x4D, 0x59, 0x53, 0xD6, 0x14, 0xF5, 0x05,
            0xFD, 0x81, 0xBF, 0xA0, 0xAF, 0xA8, 0x2B, 0xEA, 0x0A, 0x7A, 0xC2, 0x9E, 0x30, 0x06, 0x28, 0xB4, 0x7A, 0x98,
            0x04, 0x7D, 0x41, 0x5F, 0xD0, 0x17, 0x74, 0x45, 0x5D, 0xD1, 0x17, 0xF4, 0x05, 0xFD, 0x01, 0x7F, 0xC0, 0x9F,
            0xB0, 0x27, 0x6C, 0xC9, 0x9B, 0xB2, 0xA6, 0x35
        };

        public static byte[] SelectServerRequest => new byte[]
        {
            0xA0, 0x00, 0x00
        };

        public static byte[] GameServerList => new byte[]
        {
            0xA8, 0x00, 0x2E, 0x99, 0x00, 0x01, 0x00, 0x01, 0x4D, 0x6F, 0x72, 0x69, 0x61, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x7F, 0x00, 0x00, 0x01
        };

        public static byte[] EnableLockedClientFeaturesEncrypted => new byte[]
        {
            0xB6, 0xA0, 0xFE, 0xF9
        };

        public static byte[] EnableLockedClientFeatures => new byte[]
        {
            0xB9, 0x80, 0x1F
        };

        public static byte[] GameServerLoginRequest => new byte[]
        {
            0x91, 0x7F, 0x00, 0x00, 0x01, 0x61, 0x64, 0x6D, 0x69, 0x6E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x41,
            0x44, 0x4D, 0x49, 0x4E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        public static byte[] GameServerLoginRequestEncrypted => new byte[]
        {
            0x22, 0x62, 0xD9, 0x50, 0x47, 0xA6, 0x00, 0xE5, 0x14, 0x86, 0x30, 0x57, 0xED, 0x45, 0x6F, 0xFC, 0xFC, 0xA3,
            0xCF, 0x8D, 0xCF, 0x23, 0xD6, 0x53, 0x06, 0xA1, 0xD8, 0x60, 0xDD, 0xC0, 0xDC, 0x34, 0xB6, 0xA8, 0xEE, 0x4A,
            0x46, 0x1B, 0xE2, 0x3C, 0x1B, 0xD2, 0xF8, 0x7C, 0x99, 0x55, 0x7A, 0x85, 0x3B, 0xAD, 0xB9, 0x6B, 0x36, 0x97,
            0x4A, 0x7D, 0x5B, 0x2E, 0xED, 0x2E, 0x23, 0xA3, 0x12, 0xCD, 0x0D
        };

        public static byte[] ClientSpy => new byte[]
        {
            0xA4, 0x03, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x40, 0x39, 0xFD, 0x00, 0x64, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x48, 0x0F, 0xFF, 0xF9, 0xC6, 0xFF, 0xFF, 0xFF, 0xC4, 0x40, 0x10, 0x00,
            0x80, 0x00, 0x18, 0xF8, 0x48
        };

        public static byte[] LoginCharacter => new byte[]
        {
            0x5D, 0xED, 0xED, 0xED, 0xED, 0x61, 0x73, 0x64, 0x66, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xA9, 0xFE, 0x50,
            0x50
        };

        public static byte[] LoginCharacterEncrypted => new byte[]
        {
            0xE8, 0x78, 0xC2, 0x68, 0x26, 0x9B, 0xC9, 0x6B, 0xD0, 0x5A, 0x35, 0xCA, 0x15, 0xF3, 0x84, 0xDF, 0x48, 0xA3,
            0xDC, 0x13, 0x81, 0x68, 0xC1, 0xE4, 0x91, 0xFF, 0x7A, 0x42, 0xF4, 0x9D, 0xCB, 0x69, 0xB1, 0xB5, 0xB6, 0xD2,
            0x7C, 0xDF, 0x3D, 0x96, 0x64, 0x0C, 0x8A, 0x83, 0x2F, 0xF0, 0x52, 0x78, 0xDB, 0x1E, 0x41, 0xAA, 0xA7, 0x00,
            0xDF, 0xAB, 0x1A, 0x22, 0x7B, 0x79, 0xFF, 0x03, 0x16, 0x05, 0x1A, 0x54, 0xDC, 0xC4, 0xE2, 0xBD, 0xC4, 0x12,
            0x83
        };

        public static byte[] ConnectToGameServer => new byte[]
        {
            0x8C, 0x7F, 0x00, 0x00, 0x01, 0x0A, 0x21, 0x7F, 0x00, 0x00, 0x01
        };

        public static byte[] LoginSeed => new byte[] {0x7F, 0x00, 0x00, 0x01};

        public static Packet LoginSeedPacket => new Packet(PacketDefinitions.LoginSeed.Id, LoginSeed);

        public static Packet Instantiate(byte[] source)
        {
            var processingStream = new MemoryStream(source);
            var received = new byte[source.Length];

            var packetReader = new StreamPacketReader(processingStream, received);
            int packetId = packetReader.ReadByte();
            if ((packetId < 0) || (packetId > 255))
                throw new EndOfStreamException();

            var packetDefinition = PacketDefinitionRegistry.Find(packetId);
            var packetSize = packetDefinition.GetSize(packetReader);
            packetReader.ReadBytes(packetSize - packetReader.Position);
            var payload = new byte[packetSize];
            Array.Copy(received, 0, payload, 0, packetSize);

            var packet = new Packet(packetId, payload);

            return packet;
        }
    }
}