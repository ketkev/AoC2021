using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoC2021.utils;
using AoCHelper;

namespace AoC2021.Days
{
    public sealed class Day14 : BaseDay
    {
        private readonly List<string> _stringInput;

        public Day14()
        {
            _stringInput = File.ReadAllLines(InputFilePath).ToList();
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