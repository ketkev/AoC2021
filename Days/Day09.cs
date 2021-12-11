using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoC2021.utils;
using AoCHelper;

namespace AoC2021.Days
{
    class position
    {
        public int Height;
        public bool PartOfBasin;

        public position(int height)
        {
            this.Height = height;
            PartOfBasin = false;
        }
    }

    public sealed class Day09 : BaseDay
    {
        private readonly List<string> _input;
        private readonly List<List<int>> _heightMap;
        private readonly List<List<position>> _heightMapWithBool;

        public Day09()
        {
            _input = File.ReadAllLines(InputFilePath).ToList();
            _heightMap = new List<List<int>>();
            _heightMapWithBool = new List<List<position>>();

            foreach (var line in _input)
            {
                _heightMap.Add(line.Select(c => (int)char.GetNumericValue(c)).ToList());
                _heightMapWithBool.Add(line.Select(c => new position((int)char.GetNumericValue(c))).ToList());
            }
        }

        private bool IsLowPoint(int x, int y)
        {
            var current = _heightMap[y][x];

            if (y - 1 >= 0 && _heightMap[y - 1][x] <= current)
            {
                return false;
            }
            else if (x + 1 < _heightMap[y].Count && _heightMap[y][x + 1] <= current)
            {
                return false;
            }
            else if (y + 1 < _heightMap.Count && _heightMap[y + 1][x] <= current)
            {
                return false;
            }
            else if (x - 1 >= 0 && _heightMap[y][x - 1] <= current)
            {
                return false;
            }

            return true;
        }

        public override ValueTask<string> Solve_1()
        {
            var LowPoints = new List<(int x, int y)>();

            for (var y = 0; y < _heightMap.Count; y++)
            {
                for (var x = 0; x < _heightMap[y].Count; x++)
                {
                    if (IsLowPoint(x, y))
                    {
                        LowPoints.Add((x, y));
                    }
                }
            }

            var counter = 0;

            foreach ((int x, int y) coord in LowPoints)
            {
                counter += _heightMap[coord.y][coord.x] + 1;
            }

            return new ValueTask<string>(counter.ToString());
        }

        private List<Vector2Int> FindBasin(Vector2Int start)
        {
            var basin = new HashSet<Vector2Int>();
            var queue = new Queue<Vector2Int>();

            queue.Enqueue(start);

            while (queue.Count != 0)
            {
                var current = queue.Dequeue();
                _heightMapWithBool[current.y][current.x].PartOfBasin = true;
                basin.Add(current);

                if (current.y - 1 >= 0 && _heightMap[current.y - 1][current.x] < 9)
                {
                    var newCoord = new Vector2Int(current.x, current.y - 1);
                    if (!basin.Contains(newCoord) && !_heightMapWithBool[newCoord.y][newCoord.x].PartOfBasin)
                    {
                        basin.Add(newCoord);
                        queue.Enqueue(newCoord);
                    }
                }

                if (current.x + 1 < _heightMap[current.y].Count && _heightMap[current.y][current.x + 1] < 9)
                {
                    var newCoord = new Vector2Int(current.x + 1, current.y);
                    if (!basin.Contains(newCoord) && !_heightMapWithBool[newCoord.y][newCoord.x].PartOfBasin)
                    {
                        basin.Add(newCoord);
                        queue.Enqueue(newCoord);
                    }
                }

                if (current.y + 1 < _heightMap.Count && _heightMap[current.y + 1][current.x] < 9)
                {
                    var newCoord = new Vector2Int(current.x, current.y + 1);
                    if (!basin.Contains(newCoord) && !_heightMapWithBool[newCoord.y][newCoord.x].PartOfBasin)
                    {
                        basin.Add(newCoord);
                        queue.Enqueue(newCoord);
                    }
                }

                if (current.x - 1 >= 0 && _heightMap[current.y][current.x - 1] < 9)
                {
                    var newCoord = new Vector2Int(current.x - 1, current.y);
                    if (!basin.Contains(newCoord) && !_heightMapWithBool[newCoord.y][newCoord.x].PartOfBasin)
                    {
                        basin.Add(newCoord);
                        queue.Enqueue(newCoord);
                    }
                }
            }

            return basin.ToList();
        }


        public override ValueTask<string> Solve_2()
        {
            var LowPoints = new List<(int x, int y)>();

            for (var y = 0; y < _heightMap.Count; y++)
            {
                for (var x = 0; x < _heightMap[y].Count; x++)
                {
                    if (IsLowPoint(x, y))
                    {
                        LowPoints.Add((x, y));
                    }
                }
            }

            var basins = new List<List<Vector2Int>>();

            foreach (var (x, y) in LowPoints)
            {
                var basin = FindBasin(new Vector2Int(x, y));
                if (basin.Count > 0)
                {
                    basins.Add(basin);
                }
            }

            var result = basins.Select(basin => basin.Count).OrderByDescending(count => count).Take(3)
                .Aggregate(1, (cur, size) => cur *= size);

            return new ValueTask<string>(result.ToString());
        }
    }
}