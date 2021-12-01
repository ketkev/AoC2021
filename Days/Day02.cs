using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoCHelper;

namespace AoC2021.Days
{
    public sealed class Day02 : BaseDay
    {
        private readonly List<long> _heights;

        public Day02()
        {
            var input = File.ReadAllText(InputFilePath);
            _heights = input.Split('\n').Select(long.Parse).ToList();
        }

        public override ValueTask<string> Solve_1()
        {
            return new ValueTask<string>("TODO");
        }

        public override ValueTask<string> Solve_2()
        {
            return new ValueTask<string>("TODO");
        }
    }
}