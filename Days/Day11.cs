using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AoC2021.utils;
using AoCHelper;

namespace AoC2021.Days
{
    class octopus
    {
        public int Energy;
        public bool Flashed;

        public octopus(int energy, bool flashed)
        {
            this.Energy = energy;
            this.Flashed = flashed;
        }

        public override string ToString()
        {
            return Energy.ToString();
        }
    }

    public sealed class Day11 : BaseDay
    {
        private readonly List<string> _stringInput;
        private List<List<octopus>> _input;
        long flashCount = 0;

        public Day11()
        {
            _stringInput = File.ReadAllLines(InputFilePath).ToList();
            ParseInput();
        }

        private void ParseInput()
        {
            _input = new List<List<octopus>>();

            foreach (var line in _stringInput)
            {
                var row = line.Select(character => new octopus((int)char.GetNumericValue(character), false)).ToList();

                _input.Add(row);
            }
        }

        private List<Vector2Int> GetSurrounding(int x, int y)
        {
            var list = new List<Vector2Int>();

            if (y - 1 >= 0)
            {
                list.Add(new Vector2Int(x, y - 1));
            }

            if (x + 1 < _input[y].Count)
            {
                list.Add(new Vector2Int(x + 1, y));
            }

            if (y + 1 < _input.Count)
            {
                list.Add(new Vector2Int(x, y + 1));
            }

            if (x - 1 >= 0)
            {
                list.Add(new Vector2Int(x - 1, y));
            }

            // Diagonals
            if (y - 1 >= 0 && x - 1 >= 0)
            {
                list.Add(new Vector2Int(x - 1, y - 1));
            }

            if (x + 1 < _input[y].Count && y + 1 < _input.Count)
            {
                list.Add(new Vector2Int(x + 1, y + 1));
            }

            if (y + 1 < _input.Count && x - 1 >= 0)
            {
                list.Add(new Vector2Int(x - 1, y + 1));
            }

            if (x + 1 < _input[y].Count && y - 1 >= 0)
            {
                list.Add(new Vector2Int(x + 1, y - 1));
            }

            return list;
        }

        private void FlashOctopus(int x, int y)
        {
            var octopus = _input[y][x];

            octopus.Energy++;

            if (octopus.Flashed)
                return;

            if (octopus.Energy > 9)
            {
                octopus.Flashed = true;
                octopus.Energy = 0;
                flashCount++;

                var surrounding = GetSurrounding(x, y);

                foreach (var pos in surrounding)
                {
                    FlashOctopus(pos.x, pos.y);
                }
            }
        }

        public override ValueTask<string> Solve_1()
        {
            var steps = 100;

            for (int step = 0; step < steps; step++)
            {
                for (var y = 0; y < _input.Count; y++)
                {
                    for (var x = 0; x < _input[y].Count; x++)
                    {
                        FlashOctopus(x, y);
                    }
                }

                for (var y = 0; y < _input.Count; y++)
                {
                    for (var x = 0; x < _input[y].Count; x++)
                    {
                        if (_input[y][x].Flashed)
                        {
                            _input[y][x].Flashed = false;
                            _input[y][x].Energy = 0;
                        }
                    }
                }
            }

            return new ValueTask<string>(flashCount.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            ParseInput();
            var octopusCount = _input.Sum(row => row.Count);

            long step = 0;

            while (flashCount != octopusCount)
            {
                flashCount = 0;

                for (var y = 0; y < _input.Count; y++)
                {
                    for (var x = 0; x < _input[y].Count; x++)
                    {
                        FlashOctopus(x, y);
                    }
                }

                for (var y = 0; y < _input.Count; y++)
                {
                    for (var x = 0; x < _input[y].Count; x++)
                    {
                        if (_input[y][x].Flashed)
                        {
                            _input[y][x].Flashed = false;
                            _input[y][x].Energy = 0;
                        }
                    }
                }

                step++;
            }

            return new ValueTask<string>(step.ToString());
        }
    }
}