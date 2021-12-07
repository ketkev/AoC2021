using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoCHelper;

namespace AoC2021.Days
{
    public sealed class Day07 : BaseDay
    {
        private readonly List<int> _crabPositions;

        /*
         * Huge thanks to On the Unreasonable Efficacy of the Mean in Minimizing the Fuel Expenditure of Crab Submarines for the math behind today's fast implementation.
         * https://www.reddit.com/r/adventofcode/comments/rawxad/2021_day_7_part_2_i_wrote_a_paper_on_todays/
         */

        public Day07()
        {
            var input = File.ReadAllText(InputFilePath);
            _crabPositions = input.Split(',').Select(int.Parse).ToList();
        }

        public override ValueTask<string> Solve_1()
        {
            var mean = (int)Math.Round(_crabPositions.Average());

            var result = Enumerable.Range(mean - 1, 2)
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
            var mean = (int)Math.Round(_crabPositions.Average());

            var result = Enumerable.Range(mean - 1, 2)
                .Select(i => _crabPositions.Aggregate(0, (res, pos) => res + CalculateFuel(pos, i))).Min();

            return new ValueTask<string>(result.ToString());
        }
    }
}