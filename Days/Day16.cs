using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoC2021.utils;
using AoC2021.utils.graph;
using AoCHelper;

namespace AoC2021.Days
{
    class Packet
    {
        public int Version;
        public int Id;

        public bool HasValue;

        public int Value;
        public List<Packet> SubPackets;

        public Packet(int version, int id, int value)
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
                int value = 0;
                (value, remainingBits) = ParseLiteralValue(packet);
                parsedPacket = new Packet(version, id, value);
            }
            else
            {
                var subPackets = ParseSubPackets(packet);
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

        private (int, int) ParseLiteralValue(string packet)
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
            
            // TODO: Fix remaining bits
            return (Convert.ToInt32(value, 2), packet.Length - index);
        }

        private List<Packet> ParseSubPackets(string packet)
        {
            var result = new List<Packet>();

            if (packet[6] == '0')
            {
                // 15-bit number for number of bits in the subpackets
                var subPacketCountBits = packet.Substring(7, 15);
                var subPacketBitCount = Convert.ToInt32(subPacketCountBits, 2);
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
                    // TODO: Fix remaining bits
                    index = packet.Length - remainingBits;
                    result.Add(parsedPacket);
                }
            }

            return result;
        }

        public override ValueTask<string> Solve_1()
        {
            ParsePacket(ConvertToBits("EE00D40C823060"));

            return new ValueTask<string>("TODO");
        }

        public override ValueTask<string> Solve_2()
        {
            return new ValueTask<string>("TODO");
        }
    }
}