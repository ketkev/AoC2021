using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoC2021.utils;
using AoCHelper;

namespace AoC2021.Days
{
    public sealed class Day07 : BaseDay
    {
        private readonly string _input;
        private List<int> _crabPositions;

        public Day07()
        {
            _input = File.ReadAllText(InputFilePath);
            _crabPositions = _input.Split(',').Select(int.Parse).ToList();
        }

        public override ValueTask<string> Solve_1()
        {
            var min = _crabPositions.Min();
            var max = _crabPositions.Max();

            var lowest = int.MaxValue;
            var position = 0;

            for (int i = min; i < max; i++)
            {
                var fuel = _crabPositions.Aggregate(0, (res, crabPos) => res + Math.Abs(i - crabPos));

                if (fuel < lowest)
                {
                    lowest = fuel;
                    position = i;
                }
            }

            return new ValueTask<string>(lowest.ToString());
        }

        private int CalculateFuel(int start, int stop)
        {
            var steps = Math.Abs(start - stop);

            var fuel = 0;
            var step = 1;
            for (int i = 0; i < steps; i++)
            {
                fuel += step;
                step++;
            }

            return fuel;
        }

        public override ValueTask<string> Solve_2()
        {
            var min = _crabPositions.Min();
            var max = _crabPositions.Max();

            var lowest = int.MaxValue;

            for (int i = min; i < max; i++)
            {
                // Replace with n * (n + 1) / 2
                // ParallelEnumerable.Select for speed
                var fuel = _crabPositions.Aggregate(0,
                    (res, crabPos) => res + ((int)Math.Pow(Math.Abs(i - crabPos), 2) + Math.Abs(i - crabPos)) / 2);

                if (fuel < lowest)
                {
                    lowest = fuel;
                }
            }

            return new ValueTask<string>(lowest.ToString());
        }
    }
}