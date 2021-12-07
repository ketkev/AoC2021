using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoCHelper;

namespace AoC2021.Days
{
    public sealed class Day08 : BaseDay
    {
        private readonly List<int> _input;

        public Day08()
        {
            var input = File.ReadAllText(InputFilePath);
            _input = input.Split(',').Select(int.Parse).ToList();
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