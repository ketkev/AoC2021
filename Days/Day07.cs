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
        private readonly List<int> _crabPositions;

        public Day07()
        {
            var input = File.ReadAllText(InputFilePath);
            _crabPositions = input.Split(',').Select(int.Parse).ToList();
        }

        public override ValueTask<string> Solve_1()
        {
            var min = _crabPositions.Min();
            var max = _crabPositions.Max();

            var result = Enumerable.Range(min, max - min + 1).AsParallel()
                .Select(i => _crabPositions.Aggregate(0, (res, pos) => res + Math.Abs(i - pos))).Min();

            return new ValueTask<string>(result.ToString());
        }

        private static int CalculateFuel(int start, int stop)
        {
            var steps = Math.Abs(start - stop);
            return steps * (steps + 1) / 2;
        }

        public override ValueTask<string> Solve_2()
        {
            var min = _crabPositions.Min();
            var max = _crabPositions.Max();

            var result = Enumerable.Range(min, max - min + 1).AsParallel()
                .Select(i => _crabPositions.Aggregate(0, (res, pos) => res + CalculateFuel(pos, i))).Min();

            return new ValueTask<string>(result.ToString());
        }
    }
}