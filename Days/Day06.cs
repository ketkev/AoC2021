using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoC2021.utils;
using AoCHelper;

namespace AoC2021.Days
{
    public sealed class Day06 : BaseDay
    {
        private readonly string _input;
        private LinkedList<int> _fish;

        public Day06()
        {
            _input = File.ReadAllText(InputFilePath);
            _fish = new LinkedList<int>();

            _input.Split(',').ToList().ForEach(x => _fish.AddLast(int.Parse(x)));
        }

        public override ValueTask<string> Solve_1()
        {
            var daysToSimulate = 18;

            for (int day = 0; day < daysToSimulate; day++)
            {
                var current = _fish.First;

                while (current != null)
                {
                    current.Value--;

                    if (current.Value < 0)
                    {
                        _fish.AddLast(9);
                        current.Value = 6;
                    }

                    current = current.Next;
                }
            }

            return new ValueTask<string>(_fish.Count.ToString());
        }


        public override ValueTask<string> Solve_2()
        {
            var fishList = _input.Split(',').ToList().Select(int.Parse).ToList();
            var days = new List<long>(8);
            var newDays = new List<long>(8);

            for (int i = 0; i < 9; i++)
            {
                days.Add(fishList.Count(fish => fish == i));
                newDays.Add(fishList.Count(fish => fish == i));
            }

            var daysToSimulate = 256;

            for (int day = 0; day < daysToSimulate; day++)
            {
                newDays[8] = days[0];
                newDays[6] = days[0];
                newDays[0] = days[1];
                newDays[1] = days[2];
                newDays[2] = days[3];
                newDays[3] = days[4];
                newDays[4] = days[5];
                newDays[5] = days[6];
                newDays[6] += days[7];
                newDays[7] = days[8];

                for (int i = 0; i < 9; i++)
                {
                    days[i] = newDays[i];
                }
            }

            return new ValueTask<string>(days.Sum().ToString());
        }
    }
}