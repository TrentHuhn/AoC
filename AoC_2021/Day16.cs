using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AoC_2021
{
    class Day16
    {

        /// <summary>
        /// Day 16 - Packet Decoder
        /// </summary>
        public static void Run()
        {

            // Part 1 - Sum version numbers

            var start = DateTime.Now;
            var fileName = @"..\..\..\Day16\input.txt";

            Console.WriteLine($"Reading in {fileName}");
            var lines = System.IO.File.ReadAllLines(fileName);

            Console.WriteLine($"Finished reading in input file ({lines.Length} lines), parsing input...");

            if (lines.Length != 1 || string.IsNullOrEmpty(lines[0]))
                throw new Exception("Invalid input file");

            // DEBUG
            //lines[0] = "620080001611562C8802118E34";

            var bits = new BitArray(
                lines[0].Select(c => 
                    Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
                )
                .SelectMany(x => x.ToCharArray().Select(y => y == '0' ? false : true))
                .ToArray()
            );

            Packet masterPacket = DecodePackets(bits);

            // Iterate through and add up version numbers
            var versionSum = SumVersionNums(masterPacket);


            var end = DateTime.Now;
            var diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Part 1: {versionSum} ({diff} ms)");

            //  ------------------------ Part 2 ---------------------------------


            start = DateTime.Now;

            EvaluatePackets(masterPacket);
           

            end = DateTime.Now;
            diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Part 2: {masterPacket.LiteralVal} ({diff} ms)");

        }

        private static void EvaluatePackets(Packet packet)
        {
            var solution = (long)0;

            // First, recursively iterate through all subpackets that are not literal values (i.e. operators)
            var operatorSubPackets = packet.SubPackets.Where(x => x.TypeID != 4).ToList();
            foreach (var operatorPacket in operatorSubPackets)
            {
                EvaluatePackets(operatorPacket);
            }

            // We've evaluated all operand sub-packet, not calculate this packet's value
            switch(packet.TypeID)
            {
                case 0: // Sum
                    solution = packet.SubPackets.Sum(x => x.LiteralVal);
                    break;
                case 1: // Product
                    solution = packet.SubPackets.Select(x => x.LiteralVal).Aggregate((a,x) => a * x);
                    break;
                case 2: // Minimum
                    solution = packet.SubPackets.Min(x => x.LiteralVal);
                    break;
                case 3: // Maximum
                    solution = packet.SubPackets.Max(x => x.LiteralVal);
                    break;
                case 5: // Greater Than
                    if (packet.SubPackets.Count != 2)
                        throw new Exception("Greater than operand must contain exactly two sub-packets.");
                    solution = packet.SubPackets[0].LiteralVal > packet.SubPackets[1].LiteralVal ? 1 : 0;
                    break;
                case 6: // Less Than
                    if (packet.SubPackets.Count != 2)
                        throw new Exception("Greater than operand must contain exactly two sub-packets.");
                    solution = packet.SubPackets[0].LiteralVal < packet.SubPackets[1].LiteralVal ? 1 : 0;
                    break;
                case 7: // Equal to
                    if (packet.SubPackets.Count != 2)
                        throw new Exception("Greater than operand must contain exactly two sub-packets.");
                    solution = packet.SubPackets[0].LiteralVal == packet.SubPackets[1].LiteralVal ? 1 : 0;
                    break;
            }
            packet.LiteralVal = solution;

            //return packet.LiteralVal;
            
        }

        private static int SumVersionNums(Packet packet)
        {
            var versionSum = packet.Version;
            versionSum += packet.SubPackets.Sum(x => SumVersionNums(x));
            return versionSum;
        }

        private static Packet DecodePackets(BitArray bits)
        {
            // Parse Header
            var versionNum = (int)GetIntFromBitArray(CopySliceBitArray(bits, 0, 3));
            var typeNum = (int)GetIntFromBitArray(CopySliceBitArray(bits, 3, 3));

            var packet = new Packet(versionNum, typeNum);
            var curIndex = 6; // Start parsing at index 6
            if (packet.TypeID == 4) 
            {
                // This is a literal value packet
                var valueBits = new List<bool>();
                
                // Loop over groups of five-bit sets until we find one that starts with a 0
                var foundLastGroup = false;
                while(!foundLastGroup)
                {
                    //var curGroup = CopySliceBitArray(bits, curIndex, 5);
                    var curDigit = CopySliceBitArray(bits, curIndex + 1, 4);
                    valueBits.AddRange(curDigit.Cast<bool>());

                    if(!bits[curIndex])
                        foundLastGroup = true;

                    curIndex += 5;
                }

                // Parse the current digit from the valueDigits list
                var literalValue = GetIntFromBitArray(new BitArray(valueBits.ToArray()));

                packet.Remainder = bits.Length - curIndex;
                packet.LiteralVal = literalValue;

                return packet;
            }
            // Otherwise, this must be an operator packet
            if(!bits[curIndex])
            {
                // Length type ID = 0, next 15 bits represent total length (in bits) of sub-packets contained by this packet
                curIndex++;
                var subPacketBitLength = (int)GetIntFromBitArray(CopySliceBitArray(bits, curIndex, 15));
                curIndex += 15;

                var remainingBits = subPacketBitLength;
                while(remainingBits > 0 && CopySliceBitArray(bits, curIndex, remainingBits).Cast<bool>().Contains(true)) // check if we have bits remaining and they are not all 0s
                {
                    // Iterate through remaining bits and recursively call DecodePackets() to parse the subpackets
                    var newPacket = new Packet();
                    newPacket = DecodePackets(CopySliceBitArray(bits, curIndex, remainingBits));
                    curIndex += (remainingBits - newPacket.Remainder);
                    remainingBits = newPacket.Remainder;
                    packet.SubPackets.Add(newPacket);
                }
            }
            else
            {
                // Length type ID = 1, next 11 bits represent # of sub-packets *immediately* contained by this packet
                curIndex++;
                var numSubPackets = (int)GetIntFromBitArray(CopySliceBitArray(bits, curIndex, 11));
                curIndex += 11;

                var remainingPackets = numSubPackets;
                while (remainingPackets > 0) // check if we have packets remaining
                {
                    // Iterate through remaining packets and recursively call DecodePackets() to parse each subpacket
                    var newPacket = new Packet();
                    newPacket = DecodePackets(CopySliceBitArray(bits, curIndex, bits.Length - curIndex));
                    curIndex = bits.Length - newPacket.Remainder;                    
                    packet.SubPackets.Add(newPacket); 
                    remainingPackets--;
                }
            }
            
            packet.Remainder = bits.Length - curIndex; // Update the remainder for how many bits are left to process in the current parent packet

            return packet;
        }

        /// <summary>
        /// Helper function to slice a portion of a BitArray
        /// </summary>
        private static BitArray CopySliceBitArray(BitArray source, int offset, int length)
        {
            // Urgh: no CopyTo which only copies part of the BitArray
            BitArray ret = new BitArray(length);
            for (int i = 0; i < length; i++)
            {
                ret[i] = source[offset + i];
            }
            return ret;
        }

        private static long GetIntFromBitArray(BitArray bitArray)
        {
            //if (bitArray.Length > 32)
            //    throw new ArgumentException("Argument length shall be at most 32 bits.");

            //int[] array = new int[1];
            //bitArray.CopyTo(array, 0); // This assumes that the least significant bit is at the end of the array (reverse order of what we're passing)
            //return array[0];

            long value = 0;
            int count = bitArray.Count - 1;

            for (int i = 0; i < bitArray.Count; i++)
            {
                if (bitArray[i])
                    value += Convert.ToInt64(Math.Pow(2, count - i));
            }
            return value;
        }

    }

    public class Packet
    {
        public int Version { get; set; }  
        public int TypeID { get; set; }
        public long LiteralVal { get; set; }
        public bool LengthTypeID { get; set; }
        public int Remainder { get; set; }
        public List<Packet> SubPackets { get; set; }

        public Packet()
        {
            Version = -1;
            TypeID = -1;
            SubPackets = new List<Packet>();
        }
        public Packet(int version, int typeID)
        {
            Version = version;
            TypeID = typeID;
            SubPackets = new List<Packet>();
        }
    }


   
}

