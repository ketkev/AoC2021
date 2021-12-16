﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AoC2021.utils;
using AoC2021.utils.graph;
using AoCHelper;

namespace AoC2021.Days
{
    enum Operation
    {
        Sum = 0,
        Product = 1,
        Minimum = 2,
        Maximum = 3,
        Value = 4,
        Gt = 5,
        Lt = 6,
        Eq = 7
    }

    class Packet
    {
        public int Version;
        public int Id;

        public bool HasValue;

        public long Value;
        public List<Packet> SubPackets;

        public Packet(int version, int id, long value)
        {
            Version = version;
            Id = id;
            Value = value;
            HasValue = true;
        }

        public Packet(int version, int id, List<Packet> subPackets)
        {
            Version = version;
            Id = id;
            SubPackets = subPackets;
            HasValue = false;
        }

        public override string ToString()
        {
            if (HasValue)
            {
                return $"{Version} {Id}: {Value}";
            }
            else
            {
                var str = $"({Version} {Id}: [";

                foreach (var subPacket in SubPackets)
                {
                    str += $" {subPacket} ";
                }

                str += "] )";

                return str;
            }
        }
    }

    public sealed class Day16 : BaseDay
    {
        private readonly string _stringInput;

        public Day16()
        {
            _stringInput = File.ReadAllText(InputFilePath);
        }

        private string ConvertToBits(string packet)
        {
            var binaryString = string.Join("",
                packet.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
            return binaryString;
        }

        private (Packet, int) ParsePacket(string packet)
        {
            Packet parsedPacket;

            int version = GetPacketVersion(packet);
            int id = GetPacketId(packet);

            int remainingBits = 0;

            if (id == 4)
            {
                long value = 0;
                (value, remainingBits) = ParseLiteralValue(packet);
                parsedPacket = new Packet(version, id, value);
            }
            else
            {
                List<Packet> subPackets;
                (subPackets, remainingBits) = ParseSubPackets(packet);
                parsedPacket = new Packet(version, id, subPackets);
            }

            return (parsedPacket, remainingBits);
        }

        private int GetPacketVersion(string packet)
        {
            var versionBits = packet.Substring(0, 3);
            return Convert.ToInt32(versionBits, 2);
        }

        private int GetPacketId(string packet)
        {
            var versionBits = packet.Substring(3, 3);
            return Convert.ToInt32(versionBits, 2);
        }

        private (long, int) ParseLiteralValue(string packet)
        {
            var valueBits = packet.Substring(6);

            var value = "";

            var index = 0;
            var newGroupComing = true;

            while (newGroupComing)
            {
                var group = valueBits.Substring(index, 5);
                value += group.Substring(1);

                newGroupComing = group[0] == '1';
                index += 5;
            }

            return (Convert.ToInt64(value, 2), packet.Length - index - 5);
        }

        private (List<Packet>, int) ParseSubPackets(string packet)
        {
            var result = new List<Packet>();

            var remainingBitsAfterSubPacketParsing = 0;

            if (packet[6] == '0')
            {
                // 15-bit number for number of bits in the subpackets
                var subPacketCountBits = packet.Substring(7, 15);
                var subPacketBitCount = Convert.ToInt32(subPacketCountBits, 2);

                var index = 22;

                var previousRemainingBits = packet.Length - index;

                while (subPacketBitCount > 0)
                {
                    var (parsedPacket, remainingBits) = ParsePacket(packet.Substring(index));

                    subPacketBitCount -= previousRemainingBits - remainingBits + 1;
                    previousRemainingBits = remainingBits;
                    index = packet.Length - remainingBits + 1;
                    result.Add(parsedPacket);

                    remainingBitsAfterSubPacketParsing = packet.Length - index + 1;
                }
            }
            else
            {
                // 11-bit number for number of subpackets
                var subPacketCountBits = packet.Substring(7, 11);
                var subPacketCount = Convert.ToInt32(subPacketCountBits, 2);

                var index = 18;

                for (int i = 0; i < subPacketCount; i++)
                {
                    var (parsedPacket, remainingBits) = ParsePacket(packet.Substring(index));
                    index = packet.Length - remainingBits + 1;
                    result.Add(parsedPacket);
                }

                remainingBitsAfterSubPacketParsing = packet.Length - index + 1;
            }

            return (result, remainingBitsAfterSubPacketParsing);
        }

        private int CalculateVersionSum(Packet packet)
        {
            var versionSum = packet.Version;

            if (!packet.HasValue)
            {
                foreach (var subPacket in packet.SubPackets)
                {
                    versionSum += CalculateVersionSum(subPacket);
                }
            }

            return versionSum;
        }

        public override ValueTask<string> Solve_1()
        {
            var packet = ParsePacket(ConvertToBits("A0016C880162017C3686B18A3D4780")).Item1;
            Console.WriteLine(packet);
            Console.WriteLine(CalculateVersionSum(packet));

            return new ValueTask<string>(CalculateVersionSum(ParsePacket(ConvertToBits(_stringInput)).Item1)
                .ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            return new ValueTask<string>("TODO");
        }
    }
}