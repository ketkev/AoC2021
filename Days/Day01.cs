using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoCHelper;

namespace AoC2021.Days
{
    public sealed class Day01 : BaseDay
    {
        private readonly List<long> _heights;

        public Day01()
        {
            var input = File.ReadAllText(InputFilePath);
            _heights = input.Split('\n').Select(long.Parse).ToList();
        }

        public override ValueTask<string> Solve_1()
        {
            var count = 0L;
            var previous = _heights.First();

            foreach (var height in _heights)
            {
                if (height > previous)
                    count++;

                previous = height;
            }

            return new ValueTask<string>($"{count}");
        }

        private static long Calculate3SlidingWindow(IReadOnlyList<long> heights, int index)
        {
            var window = 0L;

            for (var i = index; i < index + 3 && i < heights.Count && i > 0; i++)
            {
                window += heights[i];
            }

            return window;
        }

        public override ValueTask<string> Solve_2()
        {
            var count = 0L;

            for (var i = 0; i < _heights.Count; i++)
            {
                var window = Calculate3SlidingWindow(_heights, i);
                var previousWindow = Calculate3SlidingWindow(_heights, i - 1);

                if (window > previousWindow)
                {
                    count++;
                }
            }

            return new ValueTask<string>($"{count}");
        }
    }
}