using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoC2021.utils;
using AoCHelper;

namespace AoC2021.Days
{
    public class Counts
    {
        public int ZeroCount;
        public int OneCount;

        public Counts(int zeroCount, int oneCount)
        {
            ZeroCount = zeroCount;
            OneCount = oneCount;
        }
    }

    public sealed class Day03 : BaseDay
    {
        private readonly List<string> _input;
        private List<Counts> _counts;

        public Day03()
        {
            _input = File.ReadAllLines(InputFilePath).ToList();

            _counts = new List<Counts>();

            var inputWordLength = _input.First().Length;
            for (var i = 0; i < inputWordLength; i++)
            {
                _counts.Add(new Counts(0, 0));
            }

            count_ones_and_zeros(_input);
        }

        private void count_ones_and_zeros(IEnumerable<string> list)
        {
            foreach (var counts in _counts)
            {
                counts.OneCount = 0;
                counts.ZeroCount = 0;
            }

            foreach (var row in list)
            {
                for (var i = 0; i < row.Length; i++)
                {
                    if (row[i] == '0')
                    {
                        _counts[i].ZeroCount++;
                    }
                    else
                    {
                        _counts[i].OneCount++;
                    }
                }
            }
        }


        public override ValueTask<string> Solve_1()
        {
            var gamma = 0;
            var epsilon = 0;

            for (var i = 0; i < _counts.Count; i++)
            {
                if (_counts[i].ZeroCount < _counts[i].OneCount)
                {
                    gamma += (int)Math.Pow(2, _counts.Count - 1 - i);
                }
                else if (_counts[i].ZeroCount > _counts[i].OneCount)
                {
                    epsilon += (int)Math.Pow(2, _counts.Count - 1 - i);
                }
            }

            return new ValueTask<string>((gamma * epsilon).ToString());
        }

        private LinkedList<string> ConvertToLinkedList(List<string> list)
        {
            var linkedList = new LinkedList<string>();

            foreach (var item in list)
            {
                linkedList.AddLast(item);
            }

            return linkedList;
        }

        private int FindRating(bool isOxygen)
        {
            var linkedList = ConvertToLinkedList(_input);
            var index = 0;
            while (linkedList.Count > 1)
            {
                count_ones_and_zeros(linkedList);

                var mostCommon = _counts[index].OneCount < _counts[index].ZeroCount ? '1' : '0';
                if (isOxygen)
                {
                    mostCommon = mostCommon == '0' ? '1' : '0';
                }

                var currentNode = linkedList.First;
                var nextNode = currentNode.Next;

                while (currentNode != null)
                {
                    if (currentNode.Value[index] != mostCommon)
                    {
                        linkedList.Remove(currentNode);
                    }

                    currentNode = nextNode;
                    nextNode = currentNode?.Next;
                }

                index++;
            }

            return Convert.ToInt32(linkedList.First(), 2);
        }

        public override ValueTask<string> Solve_2()
        {
            var oxygenRating = FindRating(true);
            var co2Rating = FindRating(false);

            return new ValueTask<string>((oxygenRating * co2Rating).ToString());
        }
    }
}