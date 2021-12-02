using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoC2021.utils;
using AoCHelper;

namespace AoC2021.Days
{
    struct Instruction
    {
        public string Direction;
        public int Distance;


        public Instruction(string direction, int distance)
        {
            this.Direction = direction;
            this.Distance = distance;
        }
    }

    public sealed class Day02 : BaseDay
    {
        private readonly List<Instruction> _instructions;
        private long _depth;
        private long _position;
        private long _aim;

        public Day02()
        {
            _instructions = new List<Instruction>();

            var input = File.ReadAllLines(InputFilePath);

            foreach (var str in input)
            {
                var things = str.Split(" ");
                _instructions.Add(new Instruction(things[0], int.Parse(things[1])));
            }
        }

        public override ValueTask<string> Solve_1()
        {
            _depth = 0;
            _position = 0;
            _aim = 0;

            foreach (var instruction in _instructions)
            {
                switch (instruction.Direction)
                {
                    case "forward":
                        _position += instruction.Distance;
                        break;
                    case "down":
                        _depth += instruction.Distance;
                        break;
                    case "up":
                        _depth -= instruction.Distance;
                        break;
                }
            }

            return new ValueTask<string>($"{_depth * _position}");
        }

        public override ValueTask<string> Solve_2()
        {
            _depth = 0;
            _position = 0;
            _aim = 0;

            foreach (var instruction in _instructions)
            {
                switch (instruction.Direction)
                {
                    case "forward":
                        _position += instruction.Distance;
                        _depth += _aim * instruction.Distance;
                        break;
                    case "down":
                        _aim += instruction.Distance;
                        break;
                    case "up":
                        _aim -= instruction.Distance;
                        break;
                }
            }

            return new ValueTask<string>($"{_depth * _position}");
        }
    }
}